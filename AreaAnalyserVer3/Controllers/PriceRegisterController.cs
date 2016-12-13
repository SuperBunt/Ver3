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
                houses = houses.Where(x => x.County == county);
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
            GetHistory();
            return View();         
        }

        public ActionResult GoogleMap(string address)
        {
            ViewBag.heading = address;
            return View();
        }

        // Get Price Register data for chart
        public JsonResult GetHistory()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var query = from ppr in db.PriceRegister
                      group ppr by ppr.County into grouping
                      select new
                      {
                          County = grouping.Key,
                          AvgPrice = grouping.Average(x => x.Price), 
                          DateSold = grouping.GroupBy(x => x.DateOfSale.Month)                 
                      };

            var pprList = query.ToList();
            foreach (var i in pprList)
            {
                Console.WriteLine(i.ToString());
            }

            //var houses = from p in db.PriceRegister
            //             group p by p.County
            //             select new {
            //             };

            

            // var pprList = houses.ToList();

            return Json(pprList, JsonRequestBehavior.AllowGet);
        }
    }
}
