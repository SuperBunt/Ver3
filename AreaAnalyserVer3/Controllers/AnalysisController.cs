using AreaAnalyserVer3.Migrations;
using AreaAnalyserVer3.Models;
using AreaAnalyserVer3.ViewModels;
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
        //public ActionResult Index()
        //{
        //    // Hack to debug Seed method in configuration
        //    //var conf = new Configuration();
        //    //conf.SeedDebug(db);
        //    Analysis area = new Analysis();
            
        //    ViewBag.County = new SelectList(db.Town.GroupBy(t => t.County).Select(g => g.FirstOrDefault()).ToList().OrderBy(x => x.County), "County", "County");
        //    ViewBag.TownID = new SelectList(
        //    new List<SelectListItem>
        //    {
        //        new SelectListItem{Text="Choose area", Value="id"}
        //    }
        //    , "Value", "Text"); // First parameter is the display text on screen, Second parameter is the value
        
        //    return View(area);
        //}

        //// POST
        //[HttpPost]
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
            // Gets houses in local area, allows for irish spellings
            //List<PriceRegister> houses = new List<PriceRegister>();
            //var query = from p in db.PriceRegister
            //            select p;
            //var suggestions = (from a in query
            //                   where area.Town.LocalSpellings.Any(word => a.Address.Contains(word))
            //                   select a);
            //area.HousesInArea = suggestions.OrderByDescending(x => x.DateOfSale).ToList();
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
            area.Crimes = db.AnnualReport.Where(x => x.StationId.Equals(area.Garda.StationId)).OrderByDescending(x => x.Year).ToList();
            
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

        public List<PriceRegister> GetLocalHouses(string id)
        {
            var Town = db.Town.Find(Int32.Parse(id));
            // Gets houses in local area, allows for irish spellings
            List<PriceRegister> houses = new List<PriceRegister>();
            var query = from p in db.PriceRegister
                        select p;
            var suggestions = (from a in query
                               where Town.LocalSpellings.Any(word => a.Address.Contains(word))
                               select a);

            var HousesInArea = suggestions.OrderByDescending(x => x.DateOfSale).ToList();

            return HousesInArea;
        }

        // GET: Analysis/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Analysis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Analysis/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Analysis/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Analysis/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Analysis/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Analysis/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

        // JSON for the property price register chart
        public JsonResult GetData()
        {
            db.Configuration.ProxyCreationEnabled = false;
            

            //IEnumerable<Object> query;
            var emptyList = new List<Tuple<string, int, double>>()
                .Select(t => new { Date = t.Item1, Area = t.Item2, Price = t.Item3 }).ToList();

            var query = db.PriceRegister
                .GroupBy(p => new
                {
                    p.County,
                    p.DateOfSale.Month,
                    p.DateOfSale.Year,
                    p.Price
                }).Select(y => new
                {
                    Area = y.Key.County,
                    DateSold = (y.Key.Month + "-" + y.Key.Year).ToString(),
                    PriceSold = y.Average(x => x.Price)
                })
                .OrderBy(m => m.DateSold)
                .ToList();

            return Json(emptyList, JsonRequestBehavior.AllowGet);
        }
    }
}
