using AreaAnalyserVer3.Models;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UpdatePriceRegister
{
    class PriceRegisterUpdate
    {
        private string url = "";
        public PriceRegisterUpdate()
        {
            string year = DateTime.Now.Year.ToString();

            url = "https://www.propertypriceregister.ie/website/npsra/ppr/npsra-ppr.nsf/downloads/ppr-" + year + ".csv/$file/ppr-" + year + ".csv";
        }

        // download data file from propertypriceregister.ie
        private string DownloadFile()
        {
            Console.WriteLine("Downloading file from propertypriceregister.ie");
            string error = null;
            try
            {
                string temp = Path.GetTempPath();
                // Assign GUID as name to tempfile
                var fileName = Guid.NewGuid().ToString() + ".csv";
                temp = Path.Combine(temp, fileName);
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, temp);
                }
                return temp;
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: writing PPR data file: " + e.Message);
                return error;
            }
            catch (WebException we)
            {
                Console.WriteLine("ERROR: downloading PPR data file: " + we.Message);
                return error;
            }
        }

        internal void PerformUpdate()
        {
            List<PriceRegister> pprList = new List<PriceRegister>();

            string filename = DownloadFile();

            // Get the date of the last item added to the database
            DateTime lastDate;
            using (var db = new ApplicationDbContext())
            {
                lastDate = db.PriceRegister.Max(i => i.DateOfSale);
                Console.WriteLine("Latest date in Price Register db was " + lastDate.ToShortDateString());
            }

            try
            {
                using (StreamReader reader = new StreamReader(filename, Encoding.Default))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.ReadHeader();

                    string[] charsToRemove = { "€", ",", "*" };
                    string county = "";
                    // Read each line of the csv file
                    while (csvReader.Read())
                    {
                        try
                        {
                            county = csvReader.GetField<string>(3);
                            var csvDate = csvReader.GetField<DateTime>(0);
                            int result = DateTime.Compare(csvDate, lastDate);
                            // we dont need to add any units sold before previous date
                            if (result > 0)
                            {
                                if (county.Equals("Dublin"))    //  We are only storing houses in Dublin
                                {
                                    PriceRegister toAdd = new PriceRegister();
                                    toAdd.DateOfSale = csvReader.GetField<DateTime>(0);
                                    toAdd.Address = csvReader.GetField<string>(1);
                                    toAdd.PostCode = csvReader.GetField<string>(2);
                                    toAdd.NotFullMarket = 0;
                                    var priceConvert = csvReader.GetField<string>(4);
                                    foreach (var c in charsToRemove)
                                    {
                                        priceConvert = priceConvert.Replace(c, string.Empty);
                                    }
                                    //priceConvert = priceConvert.Trim(MyChar);
                                    toAdd.Price = Convert.ToDouble(priceConvert);
                                    pprList.Add(toAdd);
                                }
                            }
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Format error reading csv file" + e);
                        }
                    }   // end csvReader.Read(), no more lines                    
                }   // end reader
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

            // Add the items to the database if the list is not empty
            #region database update
            if (pprList.Count() == 0)
            {
                Console.WriteLine("List empty, no houses updated!");
            }
            else
            {
                try
                {
                    AddToDatabase(ref pprList);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: Adding items to databse! " + e.Message);
                }
            }
            #endregion
        }

            // Add the latest items to the database
        private void AddToDatabase(ref List<PriceRegister> pprList)
        {
            Console.WriteLine("Adding items to the database. Count = " + pprList.Count());
            using(var db = new ApplicationDbContext()) { 
                foreach (var item in pprList)
                {
                    pprList.ForEach(p => db.PriceRegister.Add(p));
                }
                db.SaveChanges();
                Console.WriteLine("Finished adding items to the database");
            }  
        }
    
    }
}
