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
        public ActionResult Index()
        {
            ViewBag.Counties = from p in db.Town
                               select p.County.Distinct();
            return View();
        }

        // POST
        [HttpPost]
        public ActionResult Index(int id)
        {
            Analysis area = new Analysis();
            area.Town = db.Town.Find(id);
            area.Garda = area.Town.FindNearestGardaStation();
            area.crimes = db.AnnualReport.Where(x => x.StationId.Equals(area.Garda.StationId)).ToList();
            return View();
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
    }
}
