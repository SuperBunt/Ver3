using AreaAnalyserVer3.Migrations;
using AreaAnalyserVer3.Models;
using AreaAnalyserVer3.ViewModels;
using LinqToWiki;
using LinqToWiki.Generated;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Analysis area = new Analysis();
            ViewBag.County = new SelectList(db.Town.GroupBy(t => t.County).Select(g => g.FirstOrDefault()).ToList(), "County", "County");
            ViewBag.TownID = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem{Text="Choose area", Value="id"}
            }
            , "Value", "Text");
            area.Town = db.Town.Find(Int32.Parse(TownID));

            area.HousesInArea = GetLocalHouses(TownID);
            // Average Price over last 6 months
            double avg = 0;
            try
            {
                avg = area.HousesInArea.Average(p => (p.Price));
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e + ", No house sold in last 6 months");
            }
            area.AveragePriceLast6mths = avg;
            area.Garda = area.Town.FindNearestGardaStation();
            area.Crimes = db.AnnualReport.Where(x => x.StationId.Equals(area.Garda.StationId)).OrderBy(x => x.Year).ToList();
            var crimelist = area.Crimes.Select(r => new
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
            // Wiki components
            var wiki = new Wiki("Wiki", "https://en.wikipedia.org", "/w/api.php");
            string wiki_search = area.Town.Name + ", " + area.Town.County;

            try
            {
                area.WikiResults = (from s in wiki.Query.search(wiki_search)
                                    select new WikiContent { PageTitle = s.title, PageContent = s.snippet }).ToList().Take(5).ToList();
                foreach(var c in area.WikiResults)
                {
                    c.PageContent = c.PageContent.Replace("<span class=\"searchmatch\">", "");
                    c.PageContent = c.PageContent.Replace("</span>", "");
                }
            }
            catch(ApiErrorException e)
            {
                Console.WriteLine(e);
            }

            return View(area);
        }

        // Get Price Register data for chart
        public JsonResult GetChartData(string id)
        {
            var houses = GetLocalHouses(id);

            var monthly = houses.GroupBy(p => new
            {
                p.DateOfSale.Month,
                p.DateOfSale.Year,
                p.Price
            }).Select(y => new
            {
                //DateSold = (y.Key.Year + "-" + y.Key.Month).ToString(),
                MonthSold = y.Key.Month,
                YearSold = y.Key.Year,
                AvgPrice = y.Average(x => x.Price)
            }).
            //OrderByDescending(p => p.DateSold).
            ToList();

            var emptyList = new List<Tuple<string, double>>()
                .Select(t => new { ds = t.Item1, name = t.Item2 }).ToList();

            foreach (var row in monthly)
            {
                string month = row.MonthSold.ToString().PadLeft(2, '0');
                string date_sold = row.YearSold.ToString() + "-" + month;
                emptyList.Add(new { ds = date_sold, name = row.AvgPrice });
            }

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(emptyList);

            var list = emptyList.OrderBy(d => d.ds);

            //var emptyList = new List<Tuple<string, double>>()
            //    .Select(t => new { ds = t.Item1, name = t.Item2 }).ToList();

            //foreach (var row in monthly)
            //{
            //    emptyList.Add(new { ds = row.DateSold, name = row.AvgPrice });
            //}

            output = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            //return output;

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // Get Price Register data for chart
        public JsonResult GetCrimeData(string id)
        {
            var Town = db.Town.Find(Int32.Parse(id));

            var garda = Town.FindNearestGardaStation();
            var query = db.AnnualReport.Where(x => x.StationId.Equals(garda.StationId)).OrderBy(x => x.Year).ToList();
            var crimelist = query.Select(r => new
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

        public List<PriceRegister> GetLocalHouses(string id)
        {
            var Town = db.Town.Find(Int32.Parse(id));
            //  Town.LocalSpellings is used to allow for irish spellings of the town         
            var HousesInArea = (from a in db.PriceRegister
                               where Town.LocalSpellings.Any(word => a.Address.Contains(word))
                               select a).OrderByDescending(x => x.DateOfSale).ToList();

            //var HousesInArea = suggestions.OrderByDescending(x => x.DateOfSale).ToList();

            return HousesInArea;
        }

       
        public JsonResult GetCounties()
        {
            var counties = new SelectList(db.Town, "County", "County");
            return Json(db.Town.Select(x => x.County.Distinct()).ToList(), JsonRequestBehavior.AllowGet);
        }

        //Used for populating the drop down lists
        public JsonResult GetTowns(string county)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var query = from d in db.Town
                        where d.County.Equals(county)
                        select new { TownId = d.TownId, Name = d.Name };

            IEnumerable<Object> townList = query;
            return Json(townList);
        }

       
    }
}
