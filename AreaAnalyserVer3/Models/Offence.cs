using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class Offence
    {
        public Offence() { }
        public string TypeOfOffence { get; set; }
        public int Year { get; set; }
        public int Amount { get; set; }
        public string Station { get; set; }
        public override string ToString()
        {
            return Station + ", " + TypeOfOffence + ", " + Year + ", " + Amount;
        }
    }
}