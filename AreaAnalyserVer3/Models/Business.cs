using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class Business
    {
        public Business() { }
        // Properties
        [Key]
        public int BusinessId { get; set; }    
        public int TownId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Category  { get; set; }
        public string Phone { get; set; }
        public DbGeography GeoLocation { get; set; }
        [ForeignKey("TownId")]
        public virtual Town Town { get; set; }
    }
}