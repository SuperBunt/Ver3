using AreaAnalyserVer3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Data.Entity.Spatial;
using System.Collections.ObjectModel;

namespace AreaAnalyserVer3.ViewModels
{
    

    public class Analysis
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Analysis() { }
        // Properties
        //[System.ComponentModel.DisplayName("Address")]
        public String Address { get; set; }
        public Town Town { get; set; }
        public GardaStation Garda { get; set; }

        public List<AnnualReport> crimes { get; set; }
        
        #region Methods
        // Find nearest garda station within 125 km
        //public GardaStation FindNearestGardaStation()
        //{

        //    GardaStation gd = new GardaStation();
        //    var request = HttpWebRequest.Create(@"http://maps.googleapis.com/maps/api/geocode/json?address=" + Address + "&sensor=false");
        //    // to get fixed location you can use it as
        //    // var request = //HttpWebRequest.Create(@"http://maps.googleapis.com/maps/api/geocode/json?address=Dublin&sensor=false");

        //    request.Method = "GET";
        //    var response = request.GetResponse();

        //    using (var stream = response.GetResponseStream())
        //    {
        //        using (var reader = new StreamReader(stream))
        //        {
        //            var result = JObject.Parse(reader.ReadToEnd());

        //            var lat = (float)result.SelectToken("results[0].geometry.location.lat");
        //            var lng = (float)result.SelectToken("results[0].geometry.location.lng");


        //            var coord = GeoCoordinate.CreatePoint(lat, lng);

        //            var station = (from gs in db.GardaStation
        //                           orderby gs.Point.Distance(coord)
        //                           select gs).FirstOrDefault();
        //            // find any locations within 5 kilometers ordered by distance
        //            //db.GardaStation
        //            //.Where(loc => loc.Point.Distance(coord) < 125000)
        //            //.OrderBy(loc => loc.Point.Distance(coord))
        //            //.Select(loc => new { Address = loc.Address, Distance = loc.Point.Distance(coord) });
        //            gd = station;
        //        }
        //    }

        //    return gd;
        //}
        #endregion

    }


}