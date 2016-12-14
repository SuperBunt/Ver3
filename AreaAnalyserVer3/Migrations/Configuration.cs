namespace AreaAnalyserVer3.Migrations
{
    using CsvHelper;
    using EntityFramework.Seeder;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
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
        #region seed
        protected override void Seed(AreaAnalyserVer3.Models.ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.AnnualReport");
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.GardaStation");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.arealyser_ppr");
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "AreaAnalyserVer3.sampleData.csv";
            #region ppr
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.ReadHeader();

                    char[] MyChar = { '€', ',', '*' };
                    var houses = new List<PriceRegister>();
                    while (csvReader.Read())
                    {
                        PriceRegister toAdd = new PriceRegister();
                        toAdd.DateOfSale = csvReader.GetField<DateTime>(0);
                        var priceConvert = csvReader.GetField<string>(1);
                        // priceConvert = priceConvert.Replace("€,", "");
                        priceConvert = priceConvert.Trim(MyChar);
                        toAdd.Price = Convert.ToDouble(priceConvert);
                        toAdd.Address = csvReader.GetField<string>(2);
                        //var dateConvert = csvReader.GetField<DateTime>(0);
                        toAdd.County = csvReader.GetField<string>(3);
                        toAdd.NotFullMarket = csvReader.GetField<int>(4);
                        houses.Add(toAdd);
                        context.PriceRegister.Add(toAdd);
                        //toAdd.Price = csvReader.GetField<string>(3);
                    }

                }

            }
            context.SaveChanges();
            #endregion ppr

            #region crime
            // Use the crime data to buils annual reports
            resourceName = "AreaAnalyserVer3.crimeSample.csv";
            // List of all offences
            List<Offence> annualList = new List<Offence>();
            // Array to itearte through years
            int[] years = { 2010, 2011, 2012, 2013 };
            #region streamreader
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
                            crime.Station = csvReader.GetField<string>(1);
                            annualList.Add(crime);
                            Console.WriteLine(crime.ToString());
                        }
                    }
                }
                #endregion streamreader End
                #region StationCordinates streamreader
                // Build a list with each garda station with co-ordinates from csv file 
                resourceName = "AreaAnalyserVer3.StationCoordinates.csv";
                var stations = new List<GardaStation>();
                using (Stream stationStream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stationStream, Encoding.UTF8))
                    {
                        CsvReader csvReader = new CsvReader(reader);
                        csvReader.Configuration.WillThrowOnMissingField = false;

                        while (csvReader.Read())
                        {
                            // populate Garda stations list
                            GardaStation toAdd = new GardaStation();
                            toAdd.Address = csvReader.GetField<string>(0);
                            toAdd.Latitiude = csvReader.GetField<double>(1);
                            toAdd.Longitude = csvReader.GetField<double>(2);
                            stations.Add(toAdd);
                            Console.WriteLine(toAdd.ToString());

                        }
                    }
                }
                stations.ForEach(s => context.GardaStation.Add(s));
                context.SaveChanges();
                #endregion 

                List<AnnualReport> StationReports = new List<AnnualReport>();

                // Retrieve the table of Garda stations
                var query = from i in context.GardaStation
                            select i;
                query.ToList();
                // loop through each garda station
                #region
                foreach (var s in query)
                {
                    List<Offence> result = annualList.FindAll(x => x.Station.Contains(s.Address));
                    // Generate report for every year for each station 
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
                #endregion 
                // Add the reports to the database
                StationReports.ForEach(s => context.AnnualReport.Add(s));
                context.SaveChanges();
            }
            #endregion

        }
        #endregion

        public  void SeedDebug(ApplicationDbContext context)
        {
            Seed(context);
        }
    
    }
}
