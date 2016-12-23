using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class Town
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Town() { }

        public int TownId { get; set; }
        public string Name { get; set; }
        public string County { get; set; }
        
        public DbGeography GeoLocation { get; set; }

        #region Methods
        // Find nearest garda station
        public GardaStation FindNearestGardaStation()
        {
            GardaStation gd = new GardaStation();

            var coord = GeoLocation;

            var station = (from gs in db.GardaStation
                           orderby gs.Point.Distance(coord)
                           select gs).FirstOrDefault();
            // find any locations within 5 kilometers ordered by distance
            //db.GardaStation
            //.Where(loc => loc.Point.Distance(coord) < 125000)
            //.OrderBy(loc => loc.Point.Distance(coord))
            //.Select(loc => new { Address = loc.Address, Distance = loc.Point.Distance(coord) });
            gd = station;

            return gd;
        }

        
        #endregion
    }
}