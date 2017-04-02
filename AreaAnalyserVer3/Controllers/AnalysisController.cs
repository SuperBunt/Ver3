using AreaAnalyserVer3.Migrations;
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

namespace AreaAnalyserVer3.Controllers
{
    public class AnalysisController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: Analysis
        [HttpGet]

        public ActionResult Index(string TownID)
        {
            Analysis area = new Analysis(TownID);
            ViewBag.County = new SelectList(PriceRegister.counties);
            ViewBag.TownID = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem{Text="Choose area", Value="id"}
            }
            , "Value", "Text");
            area.Town = db.Town.Find(Int32.Parse(TownID));
            // Locate the nearest Garda station to the town
            area.Garda = area.Town.FindNearestGardaStation();
            // Retrieve the crimes stats
            area.Crimes = db.AnnualReport.Where(x => x.StationId.Equals(area.Garda.StationId)).OrderBy(x => x.Year).ToList();

            area.WikiResults = GetWiki(area.Town.Name, "Dublin");

            return View(area);
        }

        // Get Price Register data for chart
        public JsonResult GetChartData(string id)
        {
            var town = db.Town.Find(Int32.Parse(id));
            //  Town.LocalSpellings is used to allow for irish spellings of the town 

            var houses = db.PriceRegister.Where(h => h.Address.Contains(id));
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

            return Json(avg, JsonRequestBehavior.AllowGet);
        }

        // Get Price Register data for chart
        public async Task<JsonResult> GetCrimeData(string id)
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
                             select new { f.TownId, f.Name } ).FirstOrDefault();

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

        private List<WikiContent> GetWiki(string name,string county)
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
