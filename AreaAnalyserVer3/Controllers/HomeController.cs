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
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        public ActionResult Index()
        {
            // Hack to debug seed method
            //var conf = new Configuration();
            //conf.SeedDebug(db);
            

            ViewBag.County = new SelectList(db.Town.GroupBy(t => t.County).Select(g => g.FirstOrDefault()).ToList().OrderBy(x => x.County), "County", "County");
            ViewBag.TownID = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem{Text="Choose area", Value="id"}
            }
            , "Value", "Text"); // First parameter is the display text on screen, Second parameter is the value

            return View();
        }
        [HttpPost]
        public ActionResult Index(string TownID)
        {
            // Hack to debug seed method
            //var conf = new Configuration();
            //conf.SeedDebug(db);
            //Analysis area = new Analysis();

            //ViewBag.County = new SelectList(db.Town.GroupBy(t => t.County).Select(g => g.FirstOrDefault()).ToList().OrderBy(x => x.County), "County", "County");
            //ViewBag.TownID = new SelectList(
            //new List<SelectListItem>
            //{
            //    new SelectListItem{Text="Choose area", Value="id"}
            //}
            //, "Value", "Text"); // First parameter is the display text on screen, Second parameter is the value

            return RedirectToAction("Index", "Analysis", new { TownID = TownID });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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