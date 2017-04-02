using AreaAnalyserVer3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace UpdatePPR.Tasks
{
    class GetFile
    {
        public string url = "";
        public GetFile(string year)
        {
            url = "https://www.propertypriceregister.ie/website/npsra/ppr/npsra-ppr.nsf/downloads/ppr-" + year + ".csv/$file/ppr-" + year + ".csv";
        }

        // download data file from propertypriceregister.ie
        public string DownloadFile()
        {
            string error = null;
            try
            {
                string temp = Path.GetTempFileName();
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
        public List<PriceRegister> GetListFromCSV(string filename)
        {
            List<PriceRegister> pprList = new List<PriceRegister>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = filename;
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        CsvReader csvReader = new CsvReader(reader);
                        csvReader.Configuration.WillThrowOnMissingField = false;
                        csvReader.ReadHeader();

                        string[] charsToRemove = { "€", ",", "*" };
                        var list = new List<PriceRegister>();

                        var date = DateTime.Today.AddDays(-7);

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
                                    list.Add(toAdd);
                                }
                            }
                            catch (FormatException e)
                            {
                                Console.WriteLine(e);
                            }
                        }

                    }

                }
                return pprList;
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: Cannot read csv file! " + e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Cannot read csv file! " + e.Message);
                return null;
            }

        }
    }
}
