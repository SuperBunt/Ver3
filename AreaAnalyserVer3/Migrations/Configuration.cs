namespace AreaAnalyserVer3.Migrations
{
    using CsvHelper;
    using EntityFramework.Seeder;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<AreaAnalyserVer3.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AreaAnalyserVer3.Models.ApplicationDbContext context)
        {
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "AreaAnalyserVer3.sampleData.csv";
            //#region ppr
            //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //{
            //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            //    {
            //        CsvReader csvReader = new CsvReader(reader);
            //        csvReader.Configuration.WillThrowOnMissingField = false;
            //        csvReader.ReadHeader();

            //        char[] MyChar = { '€', ',', '*' };
            //        var houses = new List<PriceRegister>();
            //        while (csvReader.Read())
            //        {
            //            PriceRegister toAdd = new PriceRegister();
            //            toAdd.DateOfSale = csvReader.GetField<DateTime>(0);
            //            var priceConvert = csvReader.GetField<string>(1);
            //            // priceConvert = priceConvert.Replace("€,", "");
            //            priceConvert = priceConvert.Trim(MyChar);
            //            toAdd.Price = Convert.ToDouble(priceConvert);
            //            toAdd.Address = csvReader.GetField<string>(2);
            //            //var dateConvert = csvReader.GetField<DateTime>(0);
            //            toAdd.County = csvReader.GetField<string>(3);
            //            toAdd.NotFullMarket = csvReader.GetField<int>(4);
            //            //houses.Add(toAdd);
            //            context.PriceRegister.Add(toAdd);
            //            //toAdd.Price = csvReader.GetField<string>(3);
            //        }

            //    }

            //}
            //context.SaveChanges();
            //#endregion 

            // Add Gardastations to the database
            //#region StationCordinates 

            //resourceName = "AreaAnalyserVer3.StationCoordinates.csv";
            //var stations = new List<GardaStation>();
            //using (Stream stationStream = assembly.GetManifestResourceStream(resourceName))
            //{
            //    using (StreamReader reader = new StreamReader(stationStream, Encoding.UTF8))
            //    {
            //        CsvReader csvReader = new CsvReader(reader);
            //        csvReader.Configuration.WillThrowOnMissingField = false;
            //        double lat = 0.0;
            //        double lon = 0.0;
            //        string[] loc = new string[3];
            //        while (csvReader.Read())
            //        {
            //            // populate Garda stations list
            //            GardaStation toAdd = new GardaStation();
            //            toAdd.Address = csvReader.GetField<string>(0);
            //            loc = toAdd.Address.Split(',');
            //            toAdd.Address = loc[0];
            //            lat = csvReader.GetField<double>(1);
            //            //toAdd.Latitiude = DbGeography(getLat);
            //            lon = csvReader.GetField<double>(2);
            //            toAdd.Point = CreatePoint(lat, lon);
            //            stations.Add(toAdd);
            //            Console.WriteLine(toAdd.ToString());

            //        }
            //    }
            //}
            //stations.ForEach(s => context.GardaStation.Add(s));
            //context.SaveChanges();
            //#endregion

            // Build a list of offences, add station id then add to database 
            #region crime
            resourceName = "AreaAnalyserVer3.crimeSample.csv";
            // List of all offences
            List<Offence> listOfOffences = new List<Offence>();
            // Array to itearte through years
            int[] years = { 2010, 2011, 2012, 2013 };

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;

                    // build a list of each offence by station and year
                    while (csvReader.Read())
                    {
                        string toStrip = "";
                        string[] loc = new string[3];
                        foreach (var yr in years)
                        {
                            Offence crime = new Offence();
                            toStrip = csvReader.GetField<string>(0);
                            crime.TypeOfOffence = toStrip.Remove(0, 4);
                            // NB if the chars at the beginning of the offence are not removed they will not get counted below
                            crime.Year = yr;
                            switch (yr)
                            {
                                case 2010:
                                    crime.Amount = csvReader.GetField<int>(2);
                                    break;
                                case 2011:
                                    crime.Amount = csvReader.GetField<int>(3);
                                    break;
                                case 2012:
                                    crime.Amount = csvReader.GetField<int>(4);
                                    break;
                                case 2013:
                                    crime.Amount = csvReader.GetField<int>(5);
                                    break;

                            }
                            crime.StationAddress = csvReader.GetField<string>(1);
                            loc = crime.StationAddress.Split(',');
                            crime.StationAddress = loc[0];
                            listOfOffences.Add(crime);
                            //Console.WriteLine(crime.ToString());
                        }
                    }
                }
                //Add the offences to the database with station id
                #region offences
                var gdQuery = from i in context.GardaStation
                              select i;

                gdQuery.ToList();
                //List<Offence> tempOffenceList = new List<Offence>();

                foreach (var s in gdQuery)
                {
                    var offences = listOfOffences.Where(x => x.StationAddress.Contains(s.Address));
                    if (offences != null)
                    {
                        foreach (var off in offences)
                        {
                            off.StationId = s.StationId;
                            //tempOffenceList.Add(off);
                            context.Offence.Add(off);
                        }
                    }              
                }
                // Add the offences to the database
                //tempOffenceList.ForEach(o => context.Offence.Add(o));
                context.SaveChanges();
                #endregion
            }
            #endregion


            // Generate and annual reports
            #region AnnualReports
            List<AnnualReport> StationReports = new List<AnnualReport>();

            // Retrieve the table of Garda stations
            var query = from i in context.GardaStation
                        select i;
            query.ToList();

            // loop through each garda station and add reports          
            foreach (var s in query)
            {
                // Generate report for every year for each station
                List<Offence> result = listOfOffences.FindAll(x => x.StationAddress.Contains(s.Address));

                foreach (var yr in years)
                {
                    AnnualReport report = new AnnualReport();
                    report.Year = yr;
                    report.StationId = s.StationId;
                    foreach (var c in result)
                    {
                        if (c.Year == yr)
                        {
                            switch (c.TypeOfOffence)
                            {
                                case "Attempts/threats to murder, assaults, harassments and related offences":
                                    report.NumAttemptedMurderAssault = c.Amount;
                                    break;
                                case "Dangerous or negligent acts":
                                    report.NumDangerousActs = c.Amount;
                                    break;
                                case "Kidnapping and related offences":
                                    report.NumKidnapping = c.Amount;
                                    break;
                                case "Robbery, extortion and hijacking offences":
                                    report.NumRobbery = c.Amount;
                                    break;
                                case "Burglary and related offences":
                                    report.NumBurglary = c.Amount;
                                    break;
                                case "Theft and related offences":
                                    report.NumTheft = c.Amount;
                                    break;
                                case "Fraud, deception and related offences":
                                    report.NumFraud = c.Amount;
                                    break;
                                case "Controlled drug offences":
                                    report.NumDrugs = c.Amount;
                                    break;
                                case "Weapons and Explosives Offences":
                                    report.NumWeapons = c.Amount;
                                    break;
                                case "Damage to property and to the environment":
                                    report.NumDamage = c.Amount;
                                    break;
                                case "Public order and other social code offences":
                                    report.NumPublicOrder = c.Amount;
                                    break;
                                case "Offences against government, justice procedures and organisation of crime":
                                    report.NumGovernment = c.Amount;
                                    break;
                            }
                        }
                    }
                    StationReports.Add(report);
                }

            }
            StationReports.ForEach(s => context.AnnualReport.Add(s));
            context.SaveChanges();
            #endregion


            // Add Towns to the database
            #region Towns
            resourceName = "AreaAnalyserVer3.irishtownlands.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.Delimiter = ",";
                    csvReader.ReadHeader();

                    //var towns = new List<Town>();
                    double lat = 0.0;
                    double lon = 0.0;
                    while (csvReader.Read())
                    {
                        try {
                            Town toAdd = new Town();
                            toAdd.Name = csvReader.GetField<string>(0);
                            toAdd.County = csvReader.GetField<string>(2);
                            //toConvert = csvReader.GetField<string>(4);
                            //toConvert.Trim(single);
                            //lat = Convert.ToDouble(toConvert);
                            //toConvert = csvReader.GetField<string>(5);
                            //toConvert.Trim(single);
                            //lon = Convert.ToDouble(toConvert);
                            lat = csvReader.GetField<double>(4);
                            lon = csvReader.GetField<double>(5);
                            toAdd.GeoLocation = CreatePoint(lat, lon);
                            //towns.Add(toAdd);
                            context.Town.Add(toAdd);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        
                    }
                }

            }
            // Add towns to the database
            context.SaveChanges();
            //SaveChanges(context);

            #endregion
        }
        




    


    public void SeedDebug(ApplicationDbContext context)
    {
        Seed(context);
    }

    public static DbGeography CreatePoint(double lat, double lon)
    {
        int srid = 4326;
        string wkt = String.Format("POINT({0} {1})", lon, lat);

        return DbGeography.PointFromText(wkt, srid);
    }

    private static void SaveChanges(DbContext context)
    {
        try
        {
            context.SaveChanges();
        }
        catch (DbEntityValidationException ex)
        {
            var sb = new StringBuilder();
            foreach (var failure in ex.EntityValidationErrors)
            {
                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                foreach (var error in failure.ValidationErrors)
                {
                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    sb.AppendLine();
                }
            }
            throw new DbEntityValidationException(
                "Entity Validation Failed - errors follow:\n" +
                sb.ToString(), ex
            );
        }
    }


}
}
