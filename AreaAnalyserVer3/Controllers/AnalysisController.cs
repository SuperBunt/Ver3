
using AreaAnalyserVer3.Models;
using AreaAnalyserVer3.ViewModels;
using LinqToWiki;
using LinqToWiki.Generated;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AreaAnalyserVer3.Controllers
{
    public class AnalysisController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        //// GET: Analysis
        //[HttpGet]

        public ActionResult Index(int Id)
        {
            DateTime sixMonthsAgo = DateTime.Today.AddMonths(-6);
            try
            {
                Analysis area = new ViewModels.Analysis();
                using (var db = new ApplicationDbContext())
                {
                    area = db.Town.Where(x => x.TownId == Id)
                         .Select(
                         town => new Analysis
                         {
                             AreaName = town.Name,
                             Town = town,
                             Schools = town.Schools.ToList(),
                             NumSchools = town.Schools.Count(),
                             NearestStation = town.Garda.Name,
                             Crimes = town.Garda.Reports.ToList(),
                             Businesses = town.LocalBusinesses.ToList(),
                             NumBusinesses = town.LocalBusinesses.Count()
                         }).Single();
                }

                area.HousesInArea = db.PriceRegister.Where(x => x.Address.Contains(area.AreaName)).ToList();
                area.NumSoldinLast6mths = area.HousesInArea.Where(y => y.DateOfSale >= sixMonthsAgo).Count();
                if (area.NumSoldinLast6mths != 0)
                {
                    area.LastSixMonths = area.HousesInArea.Where(y => y.DateOfSale >= sixMonthsAgo).ToList();
                    area.AveragePriceLast6mths = area.LastSixMonths.Average(p => p.Price);                    
                }

                else
                {
                    area.AveragePriceLast6mths = 0;
                }

                // Populate map filter list with local business cateogries
                if (area.Businesses.Any(x => x.geocoded.Equals("Y "))) {
                    var types = area.Businesses
                                     .Select(t => t.Category)
                                     .Distinct();
                    var ddlist = new List<object>();
                    ddlist.Add(new { Text = "School", Value = "School" });
                    foreach (var t in types)
                    {
                        ddlist.Add( new { Text = t, Value = t } );
                    }
                    ViewBag.Categories = ddlist;
                }
                else
                {
                    // Hardcoded categories if local area has not been geocoded
                    // Used to demonstrate filter functionality for map
                    ViewBag.Categories = new List<object> {
                        new  { Text = "Butchers", Value = "Butchers" },
                        new  { Text = "Hairdresser", Value = "Hairdresser" },
                        new  { Text = "Hardware", Value = "Hardware" },
                        new  { Text = "School", Value = "School" },
                        new  { Text = "Shop", Value = "Shop" },
                        new  { Text = "Pub", Value = "Pub" }
                    };
                }

                // TODO populate database table
                area.WikiResults = GetWiki(area.Town.Name, "Dublin");

                return View(area);
            }
            catch (HttpException e)
            {
                Console.WriteLine("View not loading" + e);
            }
            return View();
        }

        //Populate the markers
        public JsonResult GetMarkers(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Town.Where(x => x.TownId == Id)
                     .Select(
                     town => new
                     {
                         latitude = town.GeoLocation.Latitude,
                         longitude = town.GeoLocation.Longitude,
                         Schools = town.Schools.ToList(),
                         Businesses = town.LocalBusinesses.ToList()
                     }).Single();


                var markers = new List<Object>();
                // Add school markers
                foreach (var s in query.Schools)
                {
                    markers.Add(new { Category = "School", Id = s.TownId, Latitude = s.GeoLocation.Latitude, Longitude = s.GeoLocation.Longitude, Description = s.Name, icon = @"../../Content/icons/school_marker.png" });
                }
                // TODO: Use this method when businesses are geocoded correctly
                if (query.Businesses.Any(x => x.geocoded.Equals("Y ")))
                {
                    var localMarker = query.Businesses.Where(g => g.geocoded.Equals("Y "));
                    foreach (var s in localMarker)
                    {
                        markers.Add(new { Category = s.Category, Latitude = s.GeoLocation.Latitude, Longitude = s.GeoLocation.Longitude, Description = s.Name, icon = @"../../Content/icons/business_marker.png" });
                    }
                }
                else
                {
                    // Only used if chosen area has not been geocoded
                    // Disgusting code to give fill map with some local businesses 
                    #region populate markers
                    var lat = query.latitude;
                    var lont = query.longitude;
                    lat += 0.003;
                    lont += 0.0005;
                    markers.Add(new { Category = "Shop", Latitude = lat, Longitude = lont, Description = "Local shop", icon = @"../../Content/icons/business_marker.png" });
                    lat += 0.0003;
                    lont += 0.00205;
                    markers.Add(new { Category = "Shop", Latitude = lat, Longitude = lont, Description = "Another Local shop", icon = @"../../Content/icons/business_marker.png" });
                    lat += 0.00066;
                    lont += 0.005002;
                    markers.Add(new { Category = "Pub", Latitude = lat, Longitude = lont, Description = "The village Inn", icon = @"../../Content/icons/business_marker.png" });
                    lat += 0.00004;
                    lont = 0.000102;
                    markers.Add(new { Category = "Pub", Latitude = lat, Longitude = lont, Description = "Castle Inn", icon = @"../../Content/icons/business_marker.png" });
                    lat = query.latitude;
                    lont = query.longitude;
                    lat += 0.00094;
                    lont -= 0.00102;
                    markers.Add(new { Category = "Pub", Latitude = lat, Longitude = lont, Description = "Fill House", icon = @"../../Content/icons/business_marker.png" });
                    lat += 0.0070045;
                    lont -= 0.0022;
                    markers.Add(new { Category = "Hairdresser", Latitude = lat, Longitude = lont, Description = "Local Hairdresser", icon = @"../../Content/icons/business_marker.png" });
                    lat += 0.0020047;
                    lont -= 0.0090233;
                    markers.Add(new { Category = "Hairdresser", Latitude = lat, Longitude = lont, Description = "Die Hard", icon = @"../../Content/icons/business_marker.png" });
                    lat += 0.0010047;
                    lont -= 0.0010233;
                    markers.Add(new { Category = "Butchers", Latitude = lat, Longitude = lont, Description = "Bills", icon = @"../../Content/icons/business_marker.png" });
                    lat = query.latitude;
                    lont = query.longitude;
                    lat -= 0.006247;
                    lont += 0.0088833;
                    markers.Add(new { Category = "Butchers", Latitude = lat, Longitude = lont, Description = "Conways", icon = @"../../Content/icons/business_marker.png" });
                    lat -= 0.0008047;
                    lont += 0.0010233;
                    markers.Add(new { Category = "Butchers", Latitude = lat, Longitude = lont, Description = "Martins butchers", icon = @"../../Content/icons/business_marker.png" });
                    lat -= 0.002247;
                    lont += 0.010233;
                    markers.Add(new { Category = "Shop", Latitude = lat, Longitude = lont, Description = "Local shop", icon = @"../../Content/icons/business_marker.png" });
                    lat = query.latitude;
                    lont = query.longitude;
                    lat += 0.008247;
                    lont -= 0.010233;
                    markers.Add(new { Category = "Shop", Latitude = lat, Longitude = lont, Description = "Another Local shop", icon = @"../../Content/icons/business_marker.png" });
                    lat -= 0.002247;
                    lont += 0.09005;
                    markers.Add(new { Category = "Hardware", Latitude = lat, Longitude = lont, Description = "Luigis", icon = @"../../Content/icons/business_marker.png" });
                    lat -= 0.02247;
                    lont += 0.000004;
                    markers.Add(new { Category = "Hardware", Latitude = lat, Longitude = lont, Description = "Sammy's", icon = @"../../Content/icons/business_marker.png" });
                    lat -= 0.0026247;
                    lont += 0.001104;
                    markers.Add(new { Category = "Shop", Latitude = lat, Longitude = lont, Description = "Centra", icon = @"../../Content/icons/business_marker.png" });
                    #endregion
                }

                return Json(new { AddressResult = markers }, JsonRequestBehavior.AllowGet);
            }
        }

        // Get local businesses data for table
        public JsonResult GetBusinesses(int id)
        {
            var result = new List<Object>();

            using (var db = new ApplicationDbContext())
            {
                var query = db.Town.Where(x => x.TownId == id)
                    .Select(
                    town => new
                    {
                        Businesses = town.LocalBusinesses.ToList()
                    }).Single();

                foreach (var b in query.Businesses)
                {
                    result.Add(new { category = b.Category, name = b.Name, phone = b.Phone, address = b.Address });
                }
            }
            result.ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        // Getprice register data for table
        public JsonResult GetHouses(string townName)
        {
            using (var db = new ApplicationDbContext())
            {
                //var houses = db.PriceRegister.Where(x => x.Address.Contains(townName)).ToList();
                var houses = (from h in db.PriceRegister
                              where h.Address.Contains(townName)
                              select new { h.DateOfSale, h.Address, h.Price }).ToArray();
                //string output = Newtonsoft.Json.JsonConvert.SerializeObject(houses);

                return Json(houses, JsonRequestBehavior.AllowGet);
            }
        }

        // Get Price Register data for chart
        public string GetChartData(string id)
        {
            using (var db = new ApplicationDbContext())
            {
                var town = db.Town.Find(Int32.Parse(id));
                //  Town.LocalSpellings is used to allow for irish spellings of the town 

                var houses = db.PriceRegister.Where(h => h.Address.Contains(town.Name)).ToArray();
                //var houses = (from a in db.PriceRegister
                //              where town.LocalSpellings.Any(word => a.Address.Contains(word) && a.County.Equals(town.County))
                //              select a).OrderByDescending(x => x.DateOfSale).ToList();

                var avg = (houses.OrderBy(x => x.DateOfSale)
                   .GroupBy(x => new { x.DateOfSale.Year, x.DateOfSale.Month })
                   .Select(p => new
                   {
                       date_sold = string.Format("{0},{1}", p.Key.Year, p.Key.Month),
                       avg_price = p.Average(i => i.Price)
                   }));

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(avg);

                return output;

                //return Json(avg, JsonRequestBehavior.AllowGet);
            }
        }

        // Get Price Register data for chart
        public JsonResult GetCrimeData(string id)
        {
            List<AnnualReport> listReports = new List<AnnualReport>();
            using (var db = new ApplicationDbContext())
            {
                var Town = db.Town.Find(Int32.Parse(id));

                listReports = Town.Garda.Reports.ToList();
                //query = db.AnnualReport.Where(x => x.StationId.Equals(garda.StationId)).OrderBy(x => x.Year).ToList();
            }

            var crimelist = listReports.Select(r => new
            {
                r.Year,
                r.NumAttemptedMurderAssault,
                r.NumBurglary,
                r.NumDamage,
                r.NumDangerousActs,
                r.NumDrugs,
                r.NumFraud,
                r.NumGovernment,
                r.NumKidnapping,
                r.NumPublicOrder,
                r.NumRobbery,
                r.NumTheft,
                r.NumWeapons
            });

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(crimelist);

            return Json(crimelist, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCounties() // XXXX Dublin
        {
            var counties = new SelectList(db.Town, "County", "County");
            return Json(db.Town.Select(x => x.Name.Distinct()).ToList(), JsonRequestBehavior.AllowGet);
        }

        //Used for populating the drop down lists       ******** XXxx Dublin
        public JsonResult GetTowns(string county)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var query = from d in db.Town
                        where d.Name.Equals(county)
                        select new { TownId = d.TownId, Name = d.Name };

            IEnumerable<Object> townList = query;
            return Json(townList);
        }


        public ActionResult FindNearestTownId(double lat, double lng)
        {
            var coord = CreatePoint(lat, lng);
            try
            {

                var query = (from f in db.Town
                             let distance = f.GeoLocation.Distance(coord)
                             where distance < 20000  // gets nearest town in 10 km radius
                             orderby distance
                             select new { f.TownId, f.Name }).FirstOrDefault();

                string id = query.TownId.ToString();

                // return RedirectToAction("Index", "Analysis", new { TownID = id });

                return Json(new { result = "Redirect", url = Url.Action("Index", "Analysis", new { TownID = id }), townName = query.Name }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //TODO: log
                return Json(new { ok = false, message = ex.Message });
            }

        }



        public static DbGeography CreatePoint(double lat, double lon)
        {
            int srid = 4326;
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }
        // required for the property chart
        public List<PriceRegister> GetLocalHouses(string id)
        {
            var Town = db.Town.Find(Int32.Parse(id));
            //  Town.LocalSpellings is used to allow for irish spellings of the town         
            var HousesInArea = (from a in db.PriceRegister
                                where Town.IrishSpelling.Any(word => a.Address.Contains(word) && a.PostCode.Equals(Town.Name))
                                select a).OrderByDescending(x => x.DateOfSale).ToList();

            return HousesInArea;
        }

        private List<WikiContent> GetWiki(string name, string county)
        {
            var wiki = new Wiki("Wiki", "https://en.wikipedia.org", "/w/api.php");
            string wiki_search = name + ", " + county;

            try
            {
                //area.WikiResults = (from s in wiki.Query.search(wiki_search)
                //                    select new WikiContent { PageTitle = s.title, PageContent = s.snippet }).ToList().Take(5).ToList();
                //foreach (var c in area.WikiResults)
                //{
                //    c.PageContent = c.PageContent.Replace("<span class=\"searchmatch\">", "");
                //    c.PageContent = c.PageContent.Replace("</span>", "");
                //}

                var query = (from s in wiki.Query.search(wiki_search)
                             select new WikiContent { PageTitle = s.title, PageContent = s.snippet }).ToList().Take(5).ToList();



                foreach (var c in query)
                {
                    c.PageContent = c.PageContent.Replace("<span class=\"searchmatch\">", "");
                    c.PageContent = c.PageContent.Replace("</span>", "");
                }
                return query;

            }
            catch (ApiErrorException e)
            {
                Console.WriteLine(e);
                return null;
            }

        }
    }
}
