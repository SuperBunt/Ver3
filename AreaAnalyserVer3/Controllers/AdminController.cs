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
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult AdminSecure()
        {
            if (Request.IsAuthenticated)
            {
                string userName = ClaimsPrincipal.Current.FindFirst("name").Value;
                string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userId))
                {
                    // Invalid principal, sign out
                    return RedirectToAction("SignOut");
                }

                // Since we cache tokens in the session, if the server restarts
                // but the browser still has a cached cookie, we may be
                // authenticated but not have a valid token cache. Check for this
                // and force signout.
                SessionTokenCache tokenCache = new SessionTokenCache(userId, HttpContext);
                if (tokenCache.Count <= 0)
                {
                    // Cache is empty, sign out
                    return RedirectToAction("SignOut");
                }
                return View();
            }
            return RedirectToAction("NoAccess");
        }

        public ActionResult NoAccess()
        {
            return View();
        }

        //GET: Admin/AddTown
        public ActionResult AddTown()
        {
            ViewBag.County = new SelectList(db.Town.GroupBy(t => t.County).Select(g => g.FirstOrDefault()).ToList().OrderBy(x => x.County), "County", "County");
            string latitude = ViewBag.Latitude;
            string longitude = ViewBag.Longitude;

            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult AddTown([Bind(Include = "TownId,County")] Town town, string longitude, string latitude)
        {
            try
            {
                int srid = 4326;
                string wkt = String.Format("POINT({0} {1})", longitude, latitude);

                town.GeoLocation = System.Data.Entity.Spatial.DbGeography.PointFromText(wkt, srid);
                db.Town.Add(town);
                return RedirectToAction("AdminSecure");
            }
            catch
            {
                return View();
            }
        }

        //public ActionResult Sent()
        //{
        //    return View();
        //}

        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                // Signal OWIN to send an authorization request to Azure
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/Admin/AdminSecure" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            if (Request.IsAuthenticated)
            {
                string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    string appId = ConfigurationManager.AppSettings["ida:AppId"];
                    // Get the user's token cache and clear it
                    SessionTokenCache tokenCache = new SessionTokenCache(userId, HttpContext);
                    tokenCache.Clear(appId);
                }
            }
            // Send an OpenID Connect sign-out request. 
            HttpContext.GetOwinContext().Authentication.SignOut(
                CookieAuthenticationDefaults.AuthenticationType);
            Response.Redirect("/");
        }

        public async Task<string> GetAccessToken()
        {
            string accessToken = null;

            // Load the app config from web.config
            string appId = ConfigurationManager.AppSettings["ida:AppId"];
            string appPassword = ConfigurationManager.AppSettings["ida:AppPassword"];
            string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
            string[] scopes = ConfigurationManager.AppSettings["ida:AppScopes"]
                .Replace(' ', ',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Get the current user's ID
            string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Get the user's token cache
                SessionTokenCache tokenCache = new SessionTokenCache(userId, HttpContext);

                ConfidentialClientApplication cca = new ConfidentialClientApplication(
                    appId, redirectUri, new ClientCredential(appPassword), tokenCache);

                // Call AcquireTokenSilentAsync, which will return the cached
                // access token if it has not expired. If it has expired, it will
                // handle using the refresh token to get a new one.
                AuthenticationResult result = await cca.AcquireTokenSilentAsync(scopes);

                accessToken = result.Token;
            }

            return accessToken;
        }



        public async Task<string> GetUserEmail()
        {
            OutlookServicesClient client =
                new OutlookServicesClient(new Uri("https://outlook.office.com/api/v2.0"), GetAccessToken);

            try
            {
                var userDetail = await client.Me.ExecuteAsync();
                return userDetail.EmailAddress;
            }
            catch (MsalException ex)
            {
                return string.Format("#ERROR#: Could not get user's email address. {0}", ex.Message);
            }
        }

        public async Task<ActionResult> Inbox()
        {
            string token = await GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                // If there's no token in the session, redirect to Home
                return Redirect("/");
            }

            string userEmail = await GetUserEmail();

            OutlookServicesClient client =
                new OutlookServicesClient(new Uri("https://outlook.office.com/api/v2.0"), GetAccessToken);

            client.Context.SendingRequest2 += new EventHandler<Microsoft.OData.Client.SendingRequest2EventArgs>(
                (sender, e) => InsertXAnchorMailboxHeader(sender, e, userEmail));

            try
            {
                var mailResults = await client.Me.Messages
                                    .OrderByDescending(m => m.ReceivedDateTime)
                                    .Take(10)
                                    .Select(m => new Models.DisplayMessage(m.Subject, m.ReceivedDateTime, m.From))
                                    .ExecuteAsync();

                return View(mailResults.CurrentPage);
            }
            catch (MsalException ex)
            {
                return RedirectToAction("Error", "Home", new { message = "ERROR retrieving messages", debug = ex.Message });
            }
        }

        private void InsertXAnchorMailboxHeader(object sender, Microsoft.OData.Client.SendingRequest2EventArgs e, string email)
        {
            e.RequestMessage.SetHeader("X-AnchorMailbox", email);
        }

        public ActionResult Error(string message, string debug)
        {
            ViewBag.Message = message;
            ViewBag.Debug = debug;
            return View("Error");
        }
    }
}

//    GET: Admin/Details/5
//    public ActionResult Details(int id)
//    {
//        return View();
//    }

//    GET: Admin/Create
//    public ActionResult Create()
//    {
//        return View();
//    }

//    POST: Admin/Create
//   [HttpPost]
//    public ActionResult Create(FormCollection collection)
//    {
//        try
//        {
//            TODO: Add insert logic here

//            return RedirectToAction("Index");
//        }
//        catch
//        {
//            return View();
//        }
//    }

//    GET: Admin/Edit/5
//    public ActionResult Edit(int id)
//    {
//        return View();
//    }

//    POST: Admin/Edit/5
//    [HttpPost]
//    public ActionResult Edit(int id, FormCollection collection)
//    {
//        try
//        {
//            TODO: Add update logic here

//            return RedirectToAction("Index");
//        }
//        catch
//        {
//            return View();
//        }
//    }

//    GET: Admin/Delete/5
//    public ActionResult Delete(int id)
//    {
//        return View();
//    }

//    POST: Admin/Delete/5
//    [HttpPost]
//    public ActionResult Delete(int id, FormCollection collection)
//    {
//        try
//        {
//            TODO: Add delete logic here

//            return RedirectToAction("Index");
//        }
//        catch
//        {
//            return View();
//        }
//    }
//}
