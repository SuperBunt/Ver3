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

        protected override void Seed(ApplicationDbContext context)
        {
            // Uncomment Debug workround in Home Controller to debug
            //InsertProperties(context);
            //InsertGardaStations(context);
            //InsertCrime(context);
            //InsertTowns(context);
            //InsertPrimarySchools(context);
            //InsertPostPrimarySchools(context);
            InsertFeederInfo(context);

        }

        


        private void InsertTowns(ApplicationDbContext context)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "AreaAnalyserVer3.irishtownlands.csv";
            using (Stream next_stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(next_stream, Encoding.UTF8))
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
                        try
                        {
                            Town toAdd = new Town();
                            toAdd.Name = csvReader.GetField<string>(0);
                            toAdd.County = csvReader.GetField<string>(2);
                            // Co-ordinates
                            lat = csvReader.GetField<double>(4);
                            lon = csvReader.GetField<double>(5);
                            toAdd.GeoLocation = CreatePoint(lat, lon);

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
            SaveChanges(context);
        }

        private void InsertCrime(ApplicationDbContext context)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "AreaAnalyserVer3.CrimeToEniscrone.csv";
            // List of all offences
            List<Offence> listOfOffences = new List<Offence>();
            // Array to itearte through years
            int[] years = { 2010, 2011, 2012, 2013, 2014, 2015, 2016 };

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    bool nextLine;
                    int id = 1;
                    // build a list of each offence by station and year
                    while (csvReader.Read())
                    {
                        nextLine = true;

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
                                case 2014:
                                    crime.Amount = csvReader.GetField<int>(6);
                                    break;
                                case 2015:
                                    crime.Amount = csvReader.GetField<int>(7);
                                    break;
                                case 2016:
                                    crime.Amount = csvReader.GetField<int>(8);
                                    break;

                            }
                            crime.StationAddress = csvReader.GetField<string>(1);

                            loc = crime.StationAddress.Split(',');
                            crime.StationAddress = loc[0];
                            //if (nextLine) { 
                            //id = (from i in context.GardaStation
                            //                   where i.Address.Equals(crime.StationAddress)
                            //                   select i.StationId).FirstOrDefault();
                            //    nextLine = false;
                            //}
                            //crime.StationId = id;
                            listOfOffences.Add(crime);
                            //context.Offence.Add(crime);
                        }
                        //context.SaveChanges();
                    }
                    AddReports(context, ref listOfOffences);
                }
            }
        }

        public void AddReports(ApplicationDbContext context, ref List<Offence> listOfOffences)
        {
            List<AnnualReport> StationReports = new List<AnnualReport>();
            // Retrieve the table of Garda stations
            var query = from i in context.GardaStation
                        select i;
            query.ToList();

            // loop through each garda station and add reports          
            foreach (var s in query)
            {
                // Generate report for every year for each station
                List<Offence> result = listOfOffences.FindAll(x => x.StationAddress.Equals(s.Address));
                int[] years = { 2010, 2011, 2012, 2013, 2014, 2015, 2016 };
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
                //SaveChanges(context);                
            }
            StationReports.ForEach(s => context.AnnualReport.Add(s));
            context.SaveChanges();
            //listOfOffences.Clear();
        }

        private void InsertGardaStations(ApplicationDbContext context)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "AreaAnalyserVer3.StationCoordinates.csv";
            var stations = new List<GardaStation>();
            using (Stream stationStream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stationStream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    double lat = 0.0;
                    double lon = 0.0;
                    string[] loc = new string[3];
                    while (csvReader.Read())
                    {
                        // populate Garda stations list
                        GardaStation toAdd = new GardaStation();
                        toAdd.Address = csvReader.GetField<string>(0);
                        loc = toAdd.Address.Split(',');
                        toAdd.Address = loc[0];
                        lat = csvReader.GetField<double>(1);
                        lon = csvReader.GetField<double>(2);
                        toAdd.Point = CreatePoint(lat, lon);
                        context.GardaStation.Add(toAdd);
                        Console.WriteLine(toAdd.ToString());
                    }
                }
            }
            //stations.ForEach(s => context.GardaStation.Add(s));
            SaveChanges(context);
        }

        private void InsertProperties(ApplicationDbContext context)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "AreaAnalyserVer3.PPR-ALL.csv";
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
                        priceConvert = priceConvert.Trim(MyChar);
                        toAdd.Price = Convert.ToDouble(priceConvert);
                        toAdd.Address = csvReader.GetField<string>(2);
                        toAdd.County = csvReader.GetField<string>(3);
                        toAdd.NotFullMarket = 0;
                        context.PriceRegister.Add(toAdd);
                    }
                }
            }
            SaveChanges(context);
        }

        public static void InsertPrimarySchools(ApplicationDbContext context)
        {
            string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Education/PrimaryGeocoded100.csv";
            string concat = "";
            double? lat, lng = 0;
            double latDbl, lngDbl = 0;
            var towns = from town in context.Town
                        select new { town.TownId, town.GeoLocation };
            towns.ToList();

            try
            {

                using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.ReadHeader();
                    csvReader.Configuration.Delimiter = "\t";
                    while (csvReader.Read())
                    {
                        try
                        {
                            School NewSchool = new School();
                            NewSchool.County = csvReader.GetField<string>(0);
                            NewSchool.Name = csvReader.GetField<string>(1);
                            string[] address = new string[4];
                            address[0] = csvReader.GetField<string>(2);
                            address[1] = csvReader.GetField<string>(3);
                            address[2] = csvReader.GetField<string>(4);
                            address[3] = csvReader.GetField<string>(5);
                            concat = string.Format($"{address[0]}, {address[1]}, {address[2]},{address[3]}");
                            NewSchool.Address = concat.TrimEnd(',');
                            NewSchool.Email = csvReader.GetField<string>(6);
                            NewSchool.DEIS = csvReader.GetField<char?>(7);
                            NewSchool.Gaeltacht = csvReader.GetField<char?>(9);
                            NewSchool.Ethos = csvReader.GetField<string>(10);
                            NewSchool.Total = csvReader.GetField<int?>(11);
                            NewSchool.Eircode = csvReader.GetField<string>(14);
                            NewSchool.Type = csvReader.GetField<string>(15);
                            if (NewSchool.Type.Equals("Special Education"))
                                NewSchool.Type = "Special";
                            else if (NewSchool.Type.Equals("Ordinary With Special Classes"))
                                NewSchool.Type = "Ord/Spec";
                            lat = csvReader.GetField<double?>(12);
                            lng = csvReader.GetField<double?>(13);
                            // Ensure the longitude and latitude are not null
                            if ((lat.HasValue && !Double.IsNaN(lat.Value)) && (lng.HasValue && !Double.IsNaN(lng.Value)))
                            {
                                latDbl = lat.Value;
                                lngDbl = lng.Value;
                                NewSchool.GeoLocation = CreatePoint(latDbl, lngDbl);
                                var query = (from f in towns
                                             let distance = f.GeoLocation.Distance(NewSchool.GeoLocation)
                                             where distance < 40000  // gets nearest town in 40 km radius
                                             orderby distance
                                             select f.TownId).FirstOrDefault();
                                NewSchool.TownId = (int?)query;
                                // Only add the school if it has a geolocation
                                context.School.Add(NewSchool);
                            }
                            

                            
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    reader.Dispose();

                }

                SaveChanges(context);

            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: Cannot read csv file! " + e.Message);
                //return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Cannot read csv file! " + e.Message);
                //return null;
            }
        }

        public static void InsertPostPrimarySchools(ApplicationDbContext context)
        {
            string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Education/sampleForTest.csv";
            string concat = "";
            double? lat, lng = 0;
            double latDbl, lngDbl = 0;
            var towns = from town in context.Town
                        select new { town.TownId, town.GeoLocation };
            towns.ToList();
            string gender = "";
            try
            {

                using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    //csvReader.ReadHeader();
                    csvReader.Configuration.Delimiter = "\t";
                    while (csvReader.Read())
                    {
                        try
                        {
                            School NewSchool = new School();

                            NewSchool.Name = csvReader.GetField<string>(2);
                            string[] address = new string[4];
                            address[0] = csvReader.GetField<string>(3);
                            address[1] = csvReader.GetField<string>(4);
                            address[2] = csvReader.GetField<string>(5);
                            address[3] = csvReader.GetField<string>(6);
                            concat = string.Format($"{address[0]}, {address[1]}, {address[2]},{address[3]}");
                            NewSchool.Address = concat.TrimEnd(',');
                            NewSchool.County = csvReader.GetField<string>(7);
                            NewSchool.Eircode = csvReader.GetField<string>(8);
                            NewSchool.Phone = csvReader.GetField<string>(10);
                            gender = csvReader.GetField<string>(11);
                            NewSchool.Gender = gender[0];
                            NewSchool.Type = csvReader.GetField<string>(12);
                            NewSchool.FeePaying = csvReader.GetField<char?>(14);
                            NewSchool.Ethos = csvReader.GetField<string>(15);
                            NewSchool.MaleEnrol = csvReader.GetField<int?>(18);
                            NewSchool.FemaleEnrol = csvReader.GetField<int?>(17);
                            NewSchool.Total = NewSchool.MaleEnrol + NewSchool.FemaleEnrol;
                            NewSchool.Email = csvReader.GetField<string>(19);

                            lat = csvReader.GetField<double?>(20);
                            lng = csvReader.GetField<double?>(21);
                            // Ensure the longitude and latitude are not null
                            if ((lat.HasValue && !Double.IsNaN(lat.Value)) && (lng.HasValue && !Double.IsNaN(lng.Value)))
                            {
                                latDbl = lat.Value;
                                lngDbl = lng.Value;
                                NewSchool.GeoLocation = CreatePoint(latDbl, lngDbl);
                                var query = (from f in towns
                                             let distance = f.GeoLocation.Distance(NewSchool.GeoLocation)
                                             where distance < 10000  // gets nearest town in 10 km radius
                                             orderby distance
                                             select f.TownId).FirstOrDefault();
                                NewSchool.TownId = (int?)query;

                            }
                            context.School.Add(NewSchool);
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    reader.Dispose();

                }

                SaveChanges(context);

            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: Cannot read csv file! " + e.Message);
                //return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: in InsertPostPrimarySchools! " + e.Message);
                //return null;
            }
        }

        // Insert the school feeder tables
        public void InsertFeederInfo(ApplicationDbContext context)
        {
            string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Education/FeederTable.csv";
            var schools = (from sch in context.School
                           select new { sch.SchoolId, sch.Name }).ToList();

            try
            {

                using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    //csvReader.ReadHeader();
                    csvReader.Configuration.Delimiter = "\t";
                    while (csvReader.Read())
                    {
                        try
                        {
                            Leaver NewLeaver = new Leaver();
                            NewLeaver.Year = 2015;
                            NewLeaver.Name = csvReader.GetField<string>(0);
                            NewLeaver.SatLeaving = csvReader.GetField<int>(1);
                            NewLeaver.UCD = csvReader.GetField<int?>(2);
                            NewLeaver.TCD = csvReader.GetField<int?>(3);
                            NewLeaver.DCU = csvReader.GetField<int?>(4);
                            NewLeaver.UL = csvReader.GetField<int?>(5);
                            NewLeaver.Maynooth = csvReader.GetField<int?>(6);
                            NewLeaver.NUIG = csvReader.GetField<int?>(7);
                            NewLeaver.UCC = csvReader.GetField<int?>(8);
                            NewLeaver.StAngelas = csvReader.GetField<int?>(9);
                            NewLeaver.QUB = csvReader.GetField<int?>(10);
                            NewLeaver.UU = csvReader.GetField<int?>(11);
                            NewLeaver.BlanchIT = csvReader.GetField<int?>(12);
                            NewLeaver.NatCol = csvReader.GetField<int?>(13);
                            NewLeaver.DIT = csvReader.GetField<int?>(14);
                            NewLeaver.ITTD = csvReader.GetField<int?>(15);
                            NewLeaver.AthloneIT = csvReader.GetField<int?>(16);
                            NewLeaver.Cork = csvReader.GetField<int?>(17);
                            NewLeaver.Dundalk = csvReader.GetField<int?>(18);
                            NewLeaver.IADT = csvReader.GetField<int?>(20);
                            NewLeaver.ITCarlow = csvReader.GetField<int?>(21);
                            NewLeaver.ITSligo = csvReader.GetField<int?>(22);
                            NewLeaver.ITTralee = csvReader.GetField<int?>(23);
                            NewLeaver.ITLetterkenny = csvReader.GetField<int?>(24);
                            NewLeaver.ITLimerick = csvReader.GetField<int?>(25);
                            NewLeaver.WIT = csvReader.GetField<int?>(26);
                            NewLeaver.Marino = csvReader.GetField<int?>(27);
                            NewLeaver.CofI = csvReader.GetField<int?>(28);
                            NewLeaver.MaryImac = csvReader.GetField<int?>(29);
                            NewLeaver.NCAD = csvReader.GetField<int?>(30);
                            NewLeaver.RCSI = csvReader.GetField<int?>(31);
                            NewLeaver.Shannon = csvReader.GetField<int?>(32);
                            NewLeaver.DEIS = 0;
                            NewLeaver.Progressed = csvReader.GetField<double>(34);
                            string[] name = new string[3];
                            name = NewLeaver.Name.Split(',');
                            NewLeaver.SchoolId = (from i in schools
                                                  where i.Name.ToUpper().Equals(name[0].ToUpper())
                                                  select i.SchoolId).Cast<int?>().FirstOrDefault();
                            
                            NewLeaver.NumAccepted = csvReader.GetField<int>(33);
                            context.Leaving.Add(NewLeaver);
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    reader.Dispose();

                }

                SaveChanges(context);

            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: Cannot read csv file! " + e.Message);
                //return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: inside InsertFeederInfo! " + e.Message);
                //return null;
            }
        }

        // Hack for debugging seed method, uncomment in Home Controller Index() to call method 
        public void SeedDebug(ApplicationDbContext context)
        {
            Seed(context);
        }

        // Method to format co-ordinate for database insertion 
        public static DbGeography CreatePoint(double lat, double lon)
        {
            int srid = 4326;
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }

        // Exception handler for insertions
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
#endregion
