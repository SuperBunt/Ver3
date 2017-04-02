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
            Houses = new List<PriceRegister>();
        }
        //Properties
        public int TownId { get; set; }
        // FK
        public int ? GardaId { get; set; }
        public string Name { get; set; }
        public string IrishSpelling { get; set; }
        public string OtherSpelling { get; set; }
        public string PostCode { get; set; }
        public int? Population { get; set; }
        public DbGeography GeoLocation { get; set; }
        
        // DB Relationships
        public virtual ICollection<Business> LocalBusinesses { get; set; }
        public virtual ICollection<School> Schools { get; set; }
        public virtual ICollection<PriceRegister> Houses { get; set; }

        [ForeignKey("GardaId")]
        public virtual GardaStation Garda { get; set; }
        #region Methods
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