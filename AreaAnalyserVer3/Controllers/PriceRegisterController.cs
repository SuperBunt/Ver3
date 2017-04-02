using AreaAnalyserVer3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AreaAnalyserVer3.Controllers
{
    public class PriceRegisterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PriceRegister
        public ActionResult search(string searchString, string county)
        {
            // string searchString = address;
            ViewBag.county = PriceRegister.counties;

            var houses = from p in db.PriceRegister
                         select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                houses = houses.Where(s => s.Address.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(county))
            {
                houses = houses.Where(x => x.Address.Contains(county));
            }
            return View(houses);
        }

        // GET: PriceRegister/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PriceRegister/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PriceRegister/Create
        [HttpPost]
        public ActionResult Create(PriceRegister ppr)
        {
            if (ModelState.IsValid)
            {
                db.PriceRegister.Add(ppr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ppr);
        }

        // GET: PriceRegister/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PriceRegister/Edit/5
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

        // GET: PriceRegister/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PriceRegister/Delete/5
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

        // GET: PriceRegister/Chart
        public ActionResult Chart()
        {
            
            var houses = (from p in db.PriceRegister
                         select p).Take(50);
            houses.OrderByDescending(p => p.DateOfSale).ToList();
            

            return View(houses);         
        }

        private void GetData(IEnumerable<PriceRegister> houses)
        {
            var monthly = houses.GroupBy(p => new
            {
                p.DateOfSale.Month,
                p.DateOfSale.Year,
                p.Price
            }).Select(y => new
            {
                DateSold = (y.Key.Month + "-" + y.Key.Year).ToString(),
                AvgPrice = y.Average(x => x.Price)
            }).ToList();

            var emptyList = new List<Tuple<string, double>>()
                .Select(t => new { ds = t.Item1, name = t.Item2 }).ToList();

            foreach (var row in monthly)
            {
                emptyList.Add(new {  ds = row.DateSold, name = row.AvgPrice });
            }
        }



        // Simple view with just map and address location
        //public ActionResult GoogleMap(string address)
        //{
        //    ViewBag.heading = address;
        //    return View();
        //}

        // Map view with Garda details
        public ActionResult GoogleMap(string address)
        {
            ViewBag.heading = address;
            return View();
        }

        // Get Price Register data for chart
        public JsonResult GetChartData()
        {
            
            db.Configuration.ProxyCreationEnabled = false;
            //IEnumerable<object> query;
            //SELECT county, avg(Price)avgPrice, CONCAT(MONTH(date_of_sale), '-', YEAR(date_of_sale)) dt
            //FROM[dbo].[arealyser_ppr]
            //group by County, MONTH(date_of_sale), YEAR(date_of_sale)
            //order by dt
            var houses = (from p in db.PriceRegister
                          select p).Take(10);
            houses.OrderByDescending(p => p.DateOfSale).ToList();

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

            var list = emptyList.OrderByDescending(d => d.ds);

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
    }
}
