using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace AreaAnalyserVer3.Models
{
    public class School
    {
        // Default Constructor
        public School() { }
        // Properties
        public int SchoolId { get; set; }
        // Foreign key
        public int ? TownId { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(5)]
        public string Level { get; set; }
        public string Type { get; set; }
        public string County { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Column(TypeName = "char")]
        [StringLength(2)]
        public string Gender { get; set; }
        [MaxLength(18)]
        public string AttendanceType { get; set; }
        public int? FeePaying { get; set; }
        public int? MaleEnrol { get; set; }
        public int? FemaleEnrol { get; set; }
        public int? Total { get; set; }
        public string Ethos { get; set; }
        [Column(TypeName = "char")]
        [StringLength(2)]
        public string  Gaeltacht { get; set; }
        [Column(TypeName = "char")]
        [StringLength(2)]
        public string DEIS { get; set; }
        [MaxLength(8)]
        public string Eircode { get; set; }
        public DbGeography GeoLocation { get; set; }

        [ForeignKey("TownId")]
        public virtual Town LocalTown { get; set; }
    }
}