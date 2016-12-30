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
        public Town(int idNew, string address)
        {
            TownId = idNew;
            Name = address;
        }

        public int TownId { get; set; }
        public string Name { get; set; }
        public string County { get; set; }
        
        // List of spelling variations of local area
        public List<String> LocalSpellings {
                get
            {
                List<String> towns = new List<String>();
                var coord = GeoLocation;
                
                var query = from d in db.Town
                            let distance = d.GeoLocation.Distance(coord)
                            where distance < 500 // list of towns in 500 meteres
                            select d.Name;

                towns = query.ToList();
                return towns;
            }
        }

        public DbGeography GeoLocation { get; set; }

        #region Methods
        // Find nearest garda station
        public GardaStation FindNearestGardaStation()
        {
            GardaStation gd = new GardaStation();

            var coord = GeoLocation;

            var q1 = from f in db.GardaStation
                     let distance = f.Point.Distance(coord)
                     where distance < 50000  // gets garda station in 500 km radius, can be modified when complete list of stations added
                     orderby distance
                     select f;

            gd = q1.FirstOrDefault();

            return gd;
        }

        

        
        #endregion
    }
}