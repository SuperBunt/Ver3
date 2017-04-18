using AreaAnalyserVer3.Models;
using AreaAnalyserVer3.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AreaAnalyserVer3.Controllers
{
    public class EducationController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //// GET: Education
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    Education Edu = new Education(784);
        //    return View(Edu);
        //}
        //// POST: Education
        //[HttpPost]
        //public ActionResult Index(string id)
        //{
        //    Education Edu = new Education(784);
        //    return View(Edu);
        //}

        // GET: Education/Details/5
        public ActionResult Details(int id)
        {
            var school = db.School.Find(id);
            if(school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // GET: Education/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Education/Create
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

        // GET: Education/Edit/5
        public ActionResult Edit(int id)
        {
            // Search for the school in the database
            var school = db.School.Find(id);
            // Return not found status code if null
            if(school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Education/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchoolId,Name,Phone,Email,Address")] School school)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(school).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Analysis", new { id = school.TownId });
                }

                return View(school);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }
        }

        // GET: Education/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Education/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // Get the pie chart info
        public JsonResult ProgressionPie(int id)
        {
            try
            {
                ApplicationDbContext data = new ApplicationDbContext();
                var feeder = data.Leaving.Where(x => x.SchoolId == (int?)id).FirstOrDefault();
                data.Dispose();
                var yes = feeder.Progressed;
                double no = 1 - yes;
                if (yes > 1)
                {
                    no = 0;
                }
                var pie = new []
                {
                   new { category = "No", val = no },
                   new { category = "Yes", val = yes }
                };

                var stats = Newtonsoft.Json.JsonConvert.SerializeObject(pie);

                return Json(pie, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("Graph Error, ");
                throw new Exception(
                    "Entity Validation Failed - errors follow:\n" +
                    e
                );
            }
        }

        // Get the bar chart info
        public JsonResult FeederTable(int id)
        {
            try
            {
                ApplicationDbContext data = new ApplicationDbContext();
                var feeder = data.Leaving.Where(x => x.SchoolId == (int?)id).FirstOrDefault();
                data.Dispose();
                // Create a list that will be serialised to correct JSON format
                var table = new [] {
                    new { College = "Athlone IT", Attendees = feeder.AthloneIT  },
                    new { College = "Blanch IT", Attendees = feeder.BlanchIT  },
                    new { College = "Nat. College of Ireland", Attendees = feeder.CofI },
                    new { College = "Cork IT", Attendees = feeder.Cork },
                    new { College = "DCU", Attendees = feeder.DCU },
                    new { College = "DIT", Attendees = feeder.DIT },
                    new { College = "Dundalk", Attendees = feeder.Dundalk },
                    new { College = "IADT", Attendees = feeder.IADT },
                    new { College = "IT Carlow", Attendees = feeder.ITCarlow },
                    new { College = "IT Letterkenny", Attendees = feeder.ITLetterkenny },
                    new { College = "IT Limerick", Attendees = feeder.ITLimerick },
                    new { College = "IT Sligo", Attendees = feeder.ITSligo },
                    new { College = "ITTD", Attendees = feeder.ITTD },
                    new { College = "ITTralee", Attendees = feeder.ITTralee },
                    new { College = "Marino Inst.", Attendees = feeder.Marino },
                    new { College = "Mary Imac.", Attendees = feeder.MaryImac },
                    new { College = "Maynooth Uni.", Attendees = feeder.Maynooth },
                    new { College = "NCAD", Attendees = feeder.NCAD },
                    new { College = "NUIG", Attendees = feeder.NUIG },
                    new { College = "QUB", Attendees = feeder.QUB },
                    new { College = "RCSI", Attendees = feeder.RCSI },
                    new { College = "Shannon", Attendees = feeder.Shannon },
                    new { College = "St. Angelas", Attendees = feeder.StAngelas },
                    new { College = "TCD", Attendees = feeder.TCD },
                    new { College = "UCC", Attendees = feeder.UCC },
                    new { College = "UCD", Attendees = feeder.UCD },
                    new { College = "UL", Attendees = feeder.UL },
                    new { College = "UU", Attendees =  feeder.UU },
                    new { College = "WIT", Attendees =  feeder.WIT}
                }.ToList();

                table.RemoveAll(x => x.Attendees == null);

                return Json(table, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("Graph Error, ");
                throw new Exception(
                    "Entity Validation Failed - errors follow:\n" +
                    e
                );
            }


        }

    }
}
