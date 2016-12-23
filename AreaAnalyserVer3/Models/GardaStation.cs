using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class GardaStation
    {
        public GardaStation() { }

        [System.ComponentModel.DataAnnotations.Key]
        public int StationId { get; set; }
        public string Address { get; set; }
        public DbGeography Point { get; set; }
        
        public virtual ICollection<AnnualReport> Reports { get; set; }
        public virtual ICollection<Offence> Crimes { get; set; }

    }
}