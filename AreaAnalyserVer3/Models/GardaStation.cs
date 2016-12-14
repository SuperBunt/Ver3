using System;
using System.Collections.Generic;
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
        //public string Division { get; set; }
        public double  Latitiude { get; set; }
        public double Longitude { get; set; }

        public virtual ICollection<AnnualReport> Reports { get; set; }
        
    }
}