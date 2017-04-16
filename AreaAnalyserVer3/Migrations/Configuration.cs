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
            //InsertProperties();

            // Garda stations MUST be inserted before the towns
            // InsertGardaStations();
            // InsertCrime();

            // Towns MUST be inserted before the schools and businesses
            // InsertTowns();
            // InsertPrimarySchools();
            // InsertPostPrimarySchools();
            // InsertFeederInfo();
            InsertBusinesses();
        }

        // Insert the garda stations
        private void InsertGardaStations()
        {
            using (var context = new ApplicationDbContext())
            {
                string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/DubStations.csv";
                var towns = (from town in context.Town
                             select new { town.GeoLocation, town.Name }).ToList();
                using (StreamReader reader = new StreamReader(file, Encoding.Default))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.ReadHeader();
                    double latDbl, lngDbl = 0.0;
                    double? lat = 0.0;
                    double? lng = 0.0;
                    string[] loc = new string[3];
                    while (csvReader.Read())
                    {
                        try
                        {
                            // populate Garda stations list
                            GardaStation toAdd = new GardaStation();
                            toAdd.Name = csvReader.GetField<string>(0);

                            latDbl = csvReader.GetField<double>(1);
                            lngDbl = csvReader.GetField<double>(2);
                            toAdd.Point = CreatePoint(latDbl, lngDbl);
                            context.GardaStation.Add(toAdd);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Reading stations: " + e);
                        }
                    }
                }

                SaveChanges(context);
            }
        }

        // Insert the local businesses from csv file to the database
        private void InsertBusinesses()
        {
            try
            {

                int buffer = 0;
                var businesses = new List<Business>();
                string resourceName = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/BusinessesForImport.csv";
                using (StreamReader reader = new StreamReader(resourceName, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.Delimiter = "\t";

                    while (csvReader.Read())
                    {
                        try
                        {
                            Business NewBusiness = new Business();
                            NewBusiness.TownId = csvReader.GetField<int>(0);
                            NewBusiness.Category = csvReader.GetField<string>(1);
                            NewBusiness.Name = csvReader.GetField<string>(2);
                            NewBusiness.Address = csvReader.GetField<string>(3);
                            NewBusiness.Phone = csvReader.GetField<string>(4);
                            NewBusiness.GeoLocation = CreatePoint(53.344926, -6.20609283);
                            NewBusiness.geocoded = "N";
                            businesses.Add(NewBusiness);
                            buffer++;
                            // Using a buffer of 500 objects, add towns to the database
                            if (buffer > 500)
                            {
                                using (var context = new ApplicationDbContext())
                                {
                                    businesses.ForEach(b => context.Business.Add(b));
                                    SaveChanges(context);
                                    buffer = 0;
                                    businesses.Clear();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Reading businesses: " + e);
                        }
                    }
                }
                // Add remaining towns to the database
                using (var context = new ApplicationDbContext())
                {
                    businesses.ForEach(b => context.Business.Add(b));              
                    SaveChanges(context);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving local businesses: " + e);
            }
        }


        // Insert the towns from csv file to the database
        private void InsertTowns()
        {
            using (var context = new ApplicationDbContext())
            {
                string resourceName = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/DubTownlands.csv";
                using (StreamReader reader = new StreamReader(resourceName, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.Delimiter = ",";

                    var stations = from t in context.GardaStation
                                   select new { t.StationId, t.Point, t.Name };

                    double lat = 0.0;
                    double lon = 0.0;
                    while (csvReader.Read())
                    {
                        try
                        {
                            Town NewTown = new Town();
                            NewTown.Name = csvReader.GetField<string>(0);
                            // Co-ordinates
                            lat = csvReader.GetField<double>(4);
                            lon = csvReader.GetField<double>(5);
                            NewTown.GeoLocation = CreatePoint(lat, lon);
                            NewTown.Population = csvReader.GetField<int?>(7);
                            // Check if any garda station names matches the town name 
                            // otherwise find the nearest station
                            if (stations.Any(x => x.Name.Equals(NewTown.Name)))
                            {
                                var garda = stations.First(t => t.Name.Equals(NewTown.Name));
                                NewTown.GardaId = garda.StationId;
                            }
                            else
                            {
                                var query = (from f in stations
                                             let distance = f.Point.Distance(NewTown.GeoLocation)
                                             where distance < 80000  // gets nearest station in 60 km radius
                                             orderby distance
                                             select f.StationId).FirstOrDefault();
                                if (query > 0 && query < stations.Count())  // Ensure the station is valid
                                    NewTown.GardaId = query;
                            }
                            context.Town.Add(NewTown);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Reading towns: " + e);

                        }
                    }
                }
                // Add towns to the database
                SaveChanges(context);
            }
        }

        private void InsertCrime()
        {
            using (var db = new ApplicationDbContext())
            {
                string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/DublinCrime.csv";
                // List of all offences
                List<Offence> listOfOffences = new List<Offence>();
                // Array to itearte through years
                int[] years = { 2010, 2011, 2012, 2013, 2014, 2015, 2016 };

                using (StreamReader reader = new StreamReader(file, Encoding.Default))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.ReadHeader();
                    // build a list of each offence by station and year
                    while (csvReader.Read())
                    {

                        string[] loc = new string[3];
                        foreach (var yr in years)
                        {
                            Offence crime = new Offence();
                            crime.TypeOfOffence = csvReader.GetField<string>(0);

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
                            listOfOffences.Add(crime);
                            //context.SaveChanges();
                        }
                        AddReports(ref listOfOffences);
                    }
                }
            }
        }
        // Insert the annual reports for each garda station
        public void AddReports(ref List<Offence> listOfOffences)
        {
            using (var context = new ApplicationDbContext())
            {
                List<AnnualReport> StationReports = new List<AnnualReport>();
                // Retrieve the table of Garda stations
                var query = from i in context.GardaStation
                            select i;
                //query.ToList();

                // loop through each garda station and add reports          
                foreach (var s in query)
                {
                    // Generate report for every year for each station
                    List<Offence> result = listOfOffences.FindAll(x => x.StationAddress.Equals(s.Name));
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

        }


        private void InsertProperties()
        {
            string resourceName = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/DubPropertiesAll.csv";
            //string resourceName = "AreaAnalyserVer3.PPR-ALL.csv";

            using (StreamReader reader = new StreamReader(resourceName, Encoding.UTF8))
            {
                CsvReader csvReader = new CsvReader(reader);
                csvReader.Configuration.WillThrowOnMissingField = false;
                //csvReader.ReadHeader();
                csvReader.Configuration.Delimiter = "\t";
                char[] MyChar = { '', ',', '*' };
                var houses = new List<PriceRegister>();
                string postCode = "";
                // Read the file row by row
                while (csvReader.Read())
                {
                    try
                    {
                        PriceRegister toAdd = new PriceRegister();
                        toAdd.DateOfSale = csvReader.GetField<DateTime>(0);
                        //var priceConvert = csvReader.GetField<string>(3);
                        //priceConvert = priceConvert.Trim(MyChar);
                        //toAdd.Price = Convert.ToDouble(priceConvert);
                        toAdd.Price = csvReader.GetField<double>(4);
                        toAdd.Address = csvReader.GetField<string>(1);
                        postCode = csvReader.GetField<string>(2);
                        if (string.IsNullOrEmpty(postCode))
                        {
                            string x = toAdd.Address.Split(',').Last().ToUpper();
                            if (x.Contains("DUBLIN"))
                                toAdd.PostCode = x;
                        }
                        toAdd.NotFullMarket = 0;
                        //db.PriceRegister.Add(toAdd);
                        houses.Add(toAdd);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Reading house list, " + e);
                    }
                    // Acts as a buffer so the stack memory doesnt grow too large
                    if (houses.Count() > 20000)
                    {
                        using (var db = new ApplicationDbContext())
                        {
                            houses.ForEach(h => db.PriceRegister.Add(h));
                            SaveChanges(db);
                            houses.Clear();
                        }
                    }
                }
                // when it reache sthe end of the file save remaing house to the database
                using (var db = new ApplicationDbContext())
                {
                    houses.ForEach(h => db.PriceRegister.Add(h));
                    SaveChanges(db);
                }
            }
        }

        public static void InsertPrimarySchools()
        {
            string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/DubPrimaryGeocoded.csv";
            using (var context = new ApplicationDbContext())
            {
                double? lat, lng = 0;
                double latDbl, lngDbl = 0;
                var towns = from town in context.Town
                            select new { town.TownId, town.Name, town.GeoLocation };
                towns.ToList();
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
                                NewSchool.Level = "prim";
                                NewSchool.County = csvReader.GetField<string>(0);
                                NewSchool.Name = csvReader.GetField<string>(1);
                                string[] address = new string[4];
                                address[0] = csvReader.GetField<string>(2);
                                address[1] = csvReader.GetField<string>(3);
                                address[2] = csvReader.GetField<string>(4);
                                address[3] = csvReader.GetField<string>(5);
                                NewSchool.Address = MakeAddress(address);
                                NewSchool.Email = csvReader.GetField<string>(6);
                                NewSchool.DEIS = csvReader.GetField<string>(9);
                                NewSchool.Ethos = csvReader.GetField<string>(10);
                                NewSchool.Total = csvReader.GetField<int?>(11);
                                NewSchool.Eircode = csvReader.GetField<string>(14);
                                NewSchool.Type = csvReader.GetField<string>(15);
                                if (NewSchool.Type.Equals("Special Education"))
                                    NewSchool.Type = "Special";
                                else if (NewSchool.Type.Equals("Ordinary With Special Classes"))
                                    NewSchool.Type = "Ordinary/Special";
                                lat = csvReader.GetField<double?>(12);
                                lng = csvReader.GetField<double?>(13);
                                // Ensure the longitude and latitude are not null
                                if ((lat.HasValue && !Double.IsNaN(lat.Value)) && (lng.HasValue && !Double.IsNaN(lng.Value)))
                                {
                                    latDbl = lat.Value;
                                    lngDbl = lng.Value;
                                    NewSchool.GeoLocation = CreatePoint(latDbl, lngDbl);
                                }
                                // Check if the address contains a town name  
                                // otherwise find the nearest town
                                if (towns.Any(x => NewSchool.Address.Contains(x.Name)))
                                {
                                    var townId = towns.First(t => NewSchool.Address.Contains(t.Name));
                                    NewSchool.TownId = townId.TownId;
                                }
                                else
                                {
                                    var query = (from f in towns
                                                 let distance = f.GeoLocation.Distance(NewSchool.GeoLocation)
                                                 where distance < 60000  // gets nearest town in 60 km radius
                                                 orderby distance
                                                 select f.TownId).FirstOrDefault();
                                    if (query > 0 && query < towns.Count())
                                        NewSchool.TownId = (int?)query; // Ensures the search returned a valid id
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
                    Console.WriteLine("ERROR: Cannot read csv file! " + e.Message);
                    //return null;
                }
            }
        }

        public static void InsertPostPrimarySchools()
        {
            string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/DubPostPrimaryGeocoded.csv";
            using (var context = new ApplicationDbContext())
            {
                double? lat, lng = 0;
                double latDbl, lngDbl = 0;
                var towns = from town in context.Town
                            select new { town.TownId, town.GeoLocation, town.Name };
                //towns.ToList();
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
                                NewSchool.Level = "post";
                                NewSchool.Name = csvReader.GetField<string>(2);
                                string[] address = new string[4];
                                address[0] = csvReader.GetField<string>(3);
                                address[1] = csvReader.GetField<string>(4);
                                address[2] = csvReader.GetField<string>(5);
                                address[3] = csvReader.GetField<string>(6);
                                NewSchool.Address = MakeAddress(address);
                                NewSchool.County = csvReader.GetField<string>(7);
                                NewSchool.Eircode = csvReader.GetField<string>(8);
                                NewSchool.Phone = csvReader.GetField<string>(10);
                                gender = csvReader.GetField<string>(11);
                                NewSchool.Gender = gender[0].ToString();
                                NewSchool.Type = csvReader.GetField<string>(12);
                                NewSchool.FeePaying = csvReader.GetField<char?>(14);
                                NewSchool.Ethos = csvReader.GetField<string>(15);
                                NewSchool.MaleEnrol = csvReader.GetField<int?>(18);
                                NewSchool.FemaleEnrol = csvReader.GetField<int?>(17);
                                NewSchool.Total = AddNullableInts(NewSchool.MaleEnrol, NewSchool.FemaleEnrol);
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
                                                 where distance < 30000  // gets nearest town in 30 km radius
                                                 orderby distance
                                                 select f.TownId).FirstOrDefault();
                                    NewSchool.TownId = (int?)query;
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
                    Console.WriteLine("ERROR: in InsertPostPrimarySchools! " + e.Message);
                    //return null;
                }
            }

        }

        // Insert the school feeder tables
        public void InsertFeederInfo()
        {
            using (var context = new ApplicationDbContext())
            {

                string file = "C:/Users/User/Documents/collegeStuff/Year4/project/Dublin/DublinFeeder.csv";
                var schools = (from sch in context.School
                               where sch.Level.Equals("post")
                               select new { sch.SchoolId, sch.Name });
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
                                NewLeaver.GMIT = csvReader.GetField<int?>(19);
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
                                NewLeaver.Progressed = csvReader.GetField<double>(34);
                                NewLeaver.Name = csvReader.GetField<string>(0);
                                NewLeaver.Name = NewLeaver.Name.TrimStart('*');
                                NewLeaver.Name = NewLeaver.Name.Split(',')[0];
                                NewLeaver.SchoolId = (from i in schools
                                                      where i.Name.ToUpper().Equals(NewLeaver.Name.ToUpper())
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

        internal static int? AddNullableInts(int? a, int? b)
        {
            if (!a.HasValue && !b.HasValue)  // guard clause, if they are both null return null
                return a;

            int aNum = a.HasValue ? a.Value : 0;  // if a has a value, assign it to aNum, if not assign 0 to aNum
            int bNum = b.HasValue ? b.Value : 0;  // same thing for b

            return aNum + bNum;
        }

        internal static string MakeAddress(string[] lines)
        {
            char[] delim = new char[] { ',' };
            lines = lines.Where(item => item != string.Empty).ToArray();
            string result = string.Join(", ", lines);

            return result;
        }
    }

}
