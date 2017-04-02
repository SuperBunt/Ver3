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

namespace HelloWorld
{
    class PPRFile
    {
        public string url = "";
        public PPRFile()
        {
            string year = DateTime.Now.Year.ToString();

            url = "https://www.propertypriceregister.ie/website/npsra/ppr/npsra-ppr.nsf/downloads/ppr-" + year + ".csv/$file/ppr-" + year + ".csv";
            
        }

        // download data file from propertypriceregister.ie
        public string DownloadFile()
        {
            string error = null;
            try
            {
                string temp = Path.GetTempPath();
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
                Console.WriteLine("ERROR: downloading PPR data file: " + e.Message);
                return error;
            }
            catch (WebException we)
            {
                Console.WriteLine("ERROR: downloading PPR data file: " + we.Message);
                return error;
            }
        }

        // return list of Priceregister objects from propertypriceregister.ie
        public void GetListFromCSV()
        {
            List<PriceRegister> pprList = new List<PriceRegister>();

            // Assembly assembly = Assembly.GetExecutingAssembly();
            //string filename = "C:/Users/User/AppData/Local/Temp/382012fa-c406-41ef-8043-31b428f64c73.csv";
             string filename = DownloadFile();
            //resourceName = FormatResourceName(assembly, resourceName);
            DateTime date = DateTime.Today.AddDays(-7);
            try
            {
                //var list = new List<PriceRegister>();
                //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(filename, Encoding.Default))
                    {
                        CsvReader csvReader = new CsvReader(reader);
                        csvReader.Configuration.WillThrowOnMissingField = false;
                        csvReader.ReadHeader();

                        string[] charsToRemove = { "€", ",", "*" };
                        

                        while (csvReader.Read())
                        {
                            try
                            {
                                var csvDate = csvReader.GetField<DateTime>(0);
                                int result = DateTime.Compare(csvDate, date);   // we dont need to add any units sold before previous date
                                if (result > 0)
                                {
                                    PriceRegister toAdd = new PriceRegister();
                                    toAdd.DateOfSale = csvReader.GetField<DateTime>(0);
                                    toAdd.Address = csvReader.GetField<string>(1);
                                    toAdd.PostCode = csvReader.GetField<string>(3);
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
                            catch (FormatException e)
                            {
                                Console.WriteLine(e);
                            }
                        }

                        //return list;
                    }

                }
                
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

            if (pprList != null)
            {
                foreach (var ppr in pprList)
                {
                    Console.WriteLine(ppr.Address);
                    // Add item to database
                }
            }
            else
            {
                Console.WriteLine("List empty, no houses updated!");
            }

        }

        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                               .Replace("\\", ".")
                                                               .Replace("/", ".");
        }
    }
}


//List<PriceRegister> list = new List<PriceRegister>();

//    Assembly assembly = Assembly.GetExecutingAssembly();
//    string resourceName = "DebugAealyser.PPR-2017-01-Dublin.csv";
//            //#region ppr
//            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
//            {
//                using (StreamReader reader = new StreamReader(stream, Encoding.Default))
//                {
//                    CsvReader csvReader = new CsvReader(reader);
//    csvReader.Configuration.WillThrowOnMissingField = false;
//                    csvReader.ReadHeader();

//                    string[] charsToRemove = { "€", ",", "*" };
//    var date = new DateTime(2017, 1, 25);

//                    while (csvReader.Read())
//                    {
//                        try
//                        {
//                            var csvDate = csvReader.GetField<DateTime>(0);
//    int result = DateTime.Compare(csvDate, date);   // we dont need to add any units sold before previous date
//                            if (result > 0)
//                            {
//                                PriceRegister toAdd = new PriceRegister();
//    toAdd.DateOfSale = csvReader.GetField<DateTime>(0);
//                                toAdd.Address = csvReader.GetField<string>(1);
//                                toAdd.County = csvReader.GetField<string>(3);
//                                toAdd.NotFullMarket = 0;
//                                var priceConvert = csvReader.GetField<string>(4);
//                                foreach (var c in charsToRemove)
//                                {
//                                    priceConvert = priceConvert.Replace(c, string.Empty);
//                                }
////priceConvert = priceConvert.Trim(MyChar);
//toAdd.Price = Convert.ToDouble(priceConvert);
//                                list.Add(toAdd);
//                            }
//                        }
//                        catch(Exception e)
//                        {
//                            Console.WriteLine(e);
//                        }
//                    }

//                }

//            }


//            foreach (var ppr in list)
//            {
//                Console.WriteLine(ppr.ToString());
//            }

//            // Console.Write(list);
//            Console.ReadKey();
//        }