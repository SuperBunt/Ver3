using AreaAnalyserVer3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AreaAnalyserVer3.Controllers
{
    public class TownController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //GET: Town
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //GET: Town/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Town/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Town/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Town/Edit/5
        public ActionResult Edit(int id)
        {
            using (var db = new Models.ApplicationDbContext())
            {
                // Search for the town in the database
                var town = db.Town.Find(id);
                // Return not found status code if null
                if (town == null)
                {
                    return HttpNotFound();
                }
                return View(town);
            }
        }

        // POST: Town/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TownId,GardaIdName,IrishSpelling,OtherSpelling,Population,PostCode,")] Town town)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(town).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Analysis", new { id = town.TownId });
                }

                return View(town);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Bad Request");
            }
        }

        // GET: Town/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Town/Delete/5
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
