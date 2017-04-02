using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class Leaver
    {
        public Leaver() { }
        public int LeaverId { get; set; }
        // Foreign key
        public int ? SchoolId { get; set; }
        [Required]
        public string Name { get; set; }
        public int SatLeaving { get; set; }
        public int NumAccepted { get; set; }
        public int Year { get; set; }
        public int? UCD { get; set; }
        public int? TCD { get; set; }
        public int? DCU { get; set; }
        public int? UL { get; set; }
        [DisplayName("Maynooth Uni")]
        public int? Maynooth { get; set; }
        public int? NUIG { get; set; }
        public int? UCC { get; set; }
        [DisplayName("St. Angelas")]
        public int? StAngelas { get; set; }
        public int? QUB { get; set; }
        public int? UU { get; set; }
        public int? BlanchIT { get; set; }
        public int? GMIT { get; set; }
        public int? NatCol { get; set; }
        public int? DIT { get; set; }
        [DisplayName("IT Tallaght")]
        public int? ITTD { get; set; }
        [DisplayName("IT Tallaght")]
        public int? AthloneIT { get; set; }
        [DisplayName("Cork IT")]
        public int? Cork { get; set; }
        [DisplayName("Dundalk IT")]
        public int? Dundalk { get; set; }
        public int? IADT { get; set; }
        [DisplayName("IT Carlow")]
        public int? ITCarlow { get; set; }
        [DisplayName("IT Sligo")]
        public int? ITSligo { get; set; }
        [DisplayName("IT Tralee")]
        public int? ITTralee { get; set; }
        [DisplayName("IT Letterkenny")]
        public int? ITLetterkenny { get; set; }
        [DisplayName("IT Limerick")]
        public int? ITLimerick { get; set; }
        public int? WIT { get; set; }
        [DisplayName("Marino Instit of Ed")]
        public int? Marino { get; set; }
        [DisplayName("C of I College of Ed")]
        public int? CofI { get; set; }
        [DisplayName("IT Tallaght")]
        public int? MaryImac { get; set; }
        public int? NCAD{ get; set; }
        public int? RCSI{ get; set; }
        public int? Shannon { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:n0}%", ApplyFormatInEditMode = true)]
        public double Progressed { get; set; }

        [ForeignKey("SchoolId")]
        public virtual School School { get; set; }

    }
}