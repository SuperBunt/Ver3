using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class Town
    {

        ApplicationDbContext db = new ApplicationDbContext();
        // Constructor
        public Town() {
            LocalBusinesses = new List<Business>();
            Schools = new List<School>();
        }
        //Properties
        public int TownId { get; set; }
        // FK
        public int ? GardaId { get; set; }
        public string Name { get; set; }
        public string IrishSpelling { get; set; }
        [Index("IX_County", IsClustered = false)]
        [MaxLength(16)]
        public string County { get; set; }
        public DbGeography GeoLocation { get; set; }
        // DB Relationships
        public virtual ICollection<Business> LocalBusinesses { get; set; }
        public virtual ICollection<School> Schools { get; set; }
        // ForeignKey reference
        public virtual GardaStation Garda { get; set; }

        #region Methods
        // List of spelling variations of local area
        public List<String> LocalSpellings
        {
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
        
        // Find nearest garda station
        public GardaStation FindNearestGardaStation()
        {
            var coord = GeoLocation;

                var station = (from f in db.GardaStation
                               let distance = f.Point.Distance(coord)
                               where distance < 50000  // gets garda station in 500 km radius, can be modified when complete list of stations added
                               orderby distance
                               select f).FirstOrDefault();
                return station;
            
        }
        #endregion
    }
}