using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BingGeocoder;
using BingMapsRESTToolkit;
using System.Net;
using AreaAnalyserVer3.Models;
using System.Threading;
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Geocoder
{
    class SerachAndUpdate
    {
    }

    public class GeocodeResult
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string errmsg { get; set; }
    }

    public class MyRequest
    {
        public int id { get; set; }
        public string Address { get; set; }
        public int MaxResults { get; set; }
    }

    public class GeocoderGo
    {
        public GeocoderGo()
        {
            RequestAndRespondver2();
        }

        private static List<MyRequest> GetRequests(int buffer)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Business.Where(x => x.geocoded.Equals("N") && x.BusinessId > 560)
                            .Select(b => new MyRequest
                            {
                                id = b.BusinessId,
                                Address = b.Address
                            }).Take(9500);

                return query.ToList();
            }
        }

        //static async Task RequestAndRespond()
        //{
        //    Console.WriteLine("Beginning RequestAndRespond");
        //    // We will create a buffer
        //    int buffer = 100;
        //    int count = 0;

        //    // We need to iterate through the requests and then store the results
        //    List<MyRequest> listOfRequest = GetRequests(buffer);
        //    List<GeocodeResult> listOfResults = new List<GeocodeResult>();

        //    // Loop through the requests
        //    foreach (var req in listOfRequest)
        //    {
        //        try
        //        {
        //            var request = new GeocodeRequest()
        //            {
        //                Culture = "en-IE",
        //                Query = req.Address,
        //                MaxResults = 1,
        //                BingMapsKey = "Ak49ymmhzPPEEj6oAi8mo_9eLxrYrCtncF-HIZEKpQtHeTaTiVr4Kb9CwMPPuKcq"
        //            };
        //            string url = request.GetRequestUrl();
        //            HttpWebRequest mapResponse = (HttpWebRequest)WebRequest.Create(request.GetRequestUrl());
        //            Console.Write(request.Query + ", ");
        //            //Process the req by using the ServiceManager.
        //            var response = await ServiceManager.GetResponseAsync(request).ConfigureAwait(false);

        //            Console.WriteLine(response.StatusDescription);
        //            if (response != null &&
        //                response.ResourceSets != null &&
        //                response.ResourceSets.Length > 0 &&
        //                response.ResourceSets[0].Resources != null &&
        //                response.ResourceSets[0].Resources.Length > 0 && 
        //                response.StatusDescription.Equals("OK"))
        //            {
        //                var result = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;
        //                Console.WriteLine($"{response.StatusDescription}, {result.Point.Coordinates[0]}, {result.Point.Coordinates[1]}");
        //                listOfResults.Add(new GeocodeResult() { Id = req.id, Lat = result.Point.Coordinates[0], Lon = result.Point.Coordinates[1] });
        //                count++;
        //                // We will use a buffer of 100 addresses to update at a time
        //                if (count > buffer)
        //                {
        //                    Console.WriteLine("Attempting update: ");
        //                    updateBusinesses(listOfResults, count);
        //                    count = 0;
        //                    listOfResults.Clear();
        //                }
        //                Console.WriteLine("Next");
        //            }
        //        }
        //        catch (InvalidOperationException e)
        //        {
        //            Console.WriteLine("Response error!!" + e);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("What went wrong?" + e);
        //        }
        //        // We dont want to overlaod the map api;
        //        Thread.Sleep(1200);
        //    }


        //}

        static void RequestAndRespondver2()
        {
            Console.WriteLine("Beginning RequestAndRespond");
            // We will create a buffer
            int buffer = 100;
            int count = 0;

            // We need to iterate through the requests and then store the results
            List<MyRequest> listOfRequest = GetRequests(buffer);
            List<GeocodeResult> listOfResults = new List<GeocodeResult>();

            // Loop through the requests
            foreach (var req in listOfRequest)
            {
                try
                {
                    var request = new GeocodeRequest()
                    {
                        Culture = "en-IE",
                        Query = req.Address,
                        MaxResults = 1,
                        BingMapsKey = "Ag_eQuNFTvuWZsNnp7_T8FWRLhRnySkPYPewU-zzmGQhdcJdmc0vcCQRtO6Sb5n_"
                    };
                    string url = request.GetRequestUrl();

                    using (var client = new WebClient())
                    {
                        string response = client.DownloadString(url);
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                        using (var es = new MemoryStream(Encoding.Unicode.GetBytes(response)))
                        {
                            var mapResponse = (ser.ReadObject(es) as Response); //Response is one of the Bing Maps DataContracts
                            Location location = (Location)mapResponse.ResourceSets.First().Resources.First();
                            if (mapResponse.StatusDescription.Equals("OK"))
                            {
                                Console.WriteLine($"{req.id}, {mapResponse.StatusDescription}, {location.Point.Coordinates[0]}, {location.Point.Coordinates[1]}");
                                listOfResults.Add(new GeocodeResult() { Id = req.id, Lat = location.Point.Coordinates[0], Lon = location.Point.Coordinates[1] });
                                count++;
                                // We will use a buffer of 100 addresses to update at a time
                                if (count > buffer)
                                {
                                    Console.WriteLine("Attempting update: ");
                                    updateBusinesses(listOfResults);
                                    count = 0;
                                    listOfResults.Clear();
                                }
                            }
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Not a valid latitude and longitude!!" +e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("What went wrong?" + e);
                }
                // We dont want to overlaod the map api;
                Thread.Sleep(800);
            }


        }

        private static void updateBusinesses(List<GeocodeResult> listOfResults)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = (from bus in db.Business
                             select bus).Where(x => x.geocoded.Equals("N"));

                foreach (var b in query)
                {
                    if (listOfResults.Any(x => x.Id == b.BusinessId))
                    {
                        var update = listOfResults.First(x => x.Id == b.BusinessId);
                        b.GeoLocation = GeoCoordinate.CreatePoint(update.Lat, update.Lon);
                        b.geocoded = "Y";
                    }
                }

                // Submit the changes to the database.
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }
            }


        }
    }
}
