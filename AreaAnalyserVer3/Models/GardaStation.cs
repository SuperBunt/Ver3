using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class GardaStation
    {
        public GardaStation() {
            Reports = new List<AnnualReport>();
            Crimes = new List<Offence>();
            Towns = new List<Town>();
        }

        [Key]
        public int StationId { get; set; }
        [DisplayName("Station")]
        public string Name { get; set; }
        public DbGeography Point { get; set; }
        
        public virtual ICollection<AnnualReport> Reports { get; set; }
        public virtual ICollection<Offence> Crimes { get; set; }
        public virtual ICollection<Town> Towns { get; set; }

    }
}