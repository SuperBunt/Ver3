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
        public ActionResult Index()
        {
            Analysis area = new Analysis();
            //var query = db.Town.Select(c => c.County.Distinct()).ToList();
            // populate area.Counites
            
            //area.Townlands.Add(new SelectListItem
            //{ Text = "-Please select-", Value = "Selects items" });
            ViewBag.County = new SelectList(db.Town.GroupBy(t => t.County).Select(g => g.FirstOrDefault()).ToList().OrderBy(x => x.County), "County", "County");
            ViewBag.TownID = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem{Text="Choose area", Value="id"}
            }
            , "Value", "Text"); // First parameter is the display text on screen, Second parameter is the value
        
            return View(area);
        }

        // POST
        [HttpPost]
        public ActionResult Index(Analysis a1, string TownID)
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
            area.Garda = area.Town.FindNearestGardaStation();
            area.Crimes = db.AnnualReport.Where(x => x.StationId.Equals(area.Garda.StationId)).OrderByDescending(x => x.Year).ToList();
            
            return View(area);
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

        
    }
}
