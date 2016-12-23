using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class Offence
    {
        public Offence() { }
        public int OffenceId { get; set; }
        public int StationId { get; set; }
        [System.ComponentModel.DisplayName("Offence")]
        public string TypeOfOffence { get; set; }
        public int Year { get; set; }
        public int Amount { get; set; }
        
        [System.ComponentModel.DisplayName("Garda station")]
        public string StationAddress { get; set; }

        public virtual GardaStation Station { get; set; }

        public override string ToString()
        {
            return Station + ", " + TypeOfOffence + ", " + Year + ", " + Amount;
        }
    }
}