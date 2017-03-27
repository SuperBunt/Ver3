using AreaAnalyserVer3.Migrations;
using AreaAnalyserVer3.Models;
using AreaAnalyserVer3.TokenStorage;
using AreaAnalyserVer3.ViewModels;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Office365.OutlookServices;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


using System.Net;
using System.Net.Mail;

namespace AreaAnalyserVer3.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        public ActionResult Index()
        {
            // Hack to debug seed method
            var conf = new Migrations.Configuration();
            conf.SeedDebug(db);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(Email model)
        {
            if (ModelState.IsValid)
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress("arealyser@outlook.com"));  // replace with valid value 
                message.From = new MailAddress("arealyser@outlook.com");  // replace with valid value
                message.Subject = model.Subject;
                //message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.Body = model.Message;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "arealyser@outlook.com",  // replace with valid value
                        Password = "DanJoe3971"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
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

        public ActionResult Error(string message, string debug)
        {
            ViewBag.Message = message;
            ViewBag.Debug = debug;
            return View("Error");
        }

        
    }
}