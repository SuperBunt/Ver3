namespace AreaAnalyserVer3.Migrations
{
    using CsvHelper;
    using EntityFramework.Seeder;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
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
        }

        public void SeedDebug(ApplicationDbContext context)
        {
            Seed(context);
        }
    }
}
