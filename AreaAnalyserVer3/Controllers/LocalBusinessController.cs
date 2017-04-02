using AngleSharp.Parser.Html;
using AreaAnalyserVer3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AreaAnalyserVer3.Controllers
{
    public class LocalBusinessController : Controller
    {
        //ApplicationDbContext db = new ApplicationDbContext();
        // GET: LocalBusiness
        public ActionResult Index()
        {
            int id = 425;
            List<Business> businesses = new List<Business>();
            using (var db = new ApplicationDbContext())
            {
                businesses = db.Business.Where(x => x.TownId == id).ToList();

                if (businesses.Count() == 0)
                {                   
                        var town = (from t in db.Town
                                    where t.TownId.Equals(id)
                                    select new { t.Name}).FirstOrDefault();
                        businesses = GetLocalBusiness(id, town.Name, "Dublin");                  
                }
                return View(businesses);
            }         
        }

        // GET: LocalBusiness/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LocalBusiness/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocalBusiness/Create
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

        // GET: LocalBusiness/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LocalBusiness/Edit/5
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

        // GET: LocalBusiness/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LocalBusiness/Delete/5
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

        private List<Business> GetLocalBusiness(int id, string name, string county)
        {
            WebRequest request = WebRequest.Create(
              "http://www.localbusinesspages.ie/area.asp?area="+name+"&county="+county);
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();

            // Create a new parser front-end (can be re-used)
            var parser = new HtmlParser();
            //Just get the DOM representation
            var document = parser.Parse(responseFromServer);

            reader.Close();
            response.Close();

            var table = document.QuerySelector("body > table > tbody");
            var rows = table.GetElementsByTagName("tr");
            List<Business> list = new List<Business>();
            var myString = "";
            //var businessInfo = new Business();
            if (rows.Count() > 4)
            {

                foreach (var r in rows)
                {

                    if (r.Children.Length == 4)
                    {
                        var businessInfo = new Business();
                        businessInfo.TownId = id;
                        var cells = r.Children.ToList();
                        myString = cells[0].TextContent;
                        businessInfo.Category = myString.Replace("\n", "").Trim();
                        myString = cells[1].TextContent;
                        businessInfo.Name = myString.Replace("\n", "").Trim();
                        myString = cells[2].TextContent;
                        businessInfo.Address = myString.Replace("\n", "").Trim();
                        myString = cells[3].TextContent;
                        businessInfo.Phone = myString.Replace("\n", "").Trim();
                        list.Add(businessInfo);
                    }
                    
                }
                // Remove first 3 elements
                int remove = Math.Min(list.Count, 3);
                list.RemoveRange(0, remove);
                return list;
                //list.ForEach(b => db.Business.Add(b));
            }
            return null;
        }
    }
}
