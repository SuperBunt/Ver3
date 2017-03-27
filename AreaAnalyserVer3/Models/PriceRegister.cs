using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AreaAnalyserVer3.Models
{
    [Table("PriceRegister")]
    public class PriceRegister
    {
        public PriceRegister() { }

        // properties
        [Column("id")]
        public int PriceRegisterId { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("price")]
        [DisplayFormat(DataFormatString = " {0:N}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }
        [DisplayName("Date sold")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column("date_of_sale")]
        public DateTime DateOfSale { get; set; }
        [Column("county")]
        [Index("IX_County", IsClustered = false)]
        [MaxLength(16)]
        public string County { get; set; }
        [Column("not_full_market")]
        public int NotFullMarket { get; set; }
        public static SelectList counties = new SelectList(new[]
            {
                new {id="Carlow", Name="Carlow"},
                new {id="Clare", Name="Clare"},
                new {id="Cavan", Name="Cavan"},
                new {id="Cork", Name="Cork"},
                new {id="Donegal", Name="Donegal"},
                new {id="Dublin", Name="Dublin"},
                new {id="Galway", Name="Galway"},
                new {id="Kerry", Name="Kerry"},
                new {id="Kildare", Name="Kildare"},
                new {id="Kilkenny", Name="Kilkenny"},
                new {id="Laois", Name="Laois"},
                new {id="Leitrim", Name="Leitrim"},
                new {id="Limerick", Name="Limerick"},
                new {id="Longford", Name="Longford"},
                new {id="Louth", Name="Louth"},
                new {id="Mayo", Name="Mayo"},
                new {id="Meath", Name="Meath"},
                new {id="Monaghan", Name="Monaghan"},
                new {id="Offaly", Name="Offaly"},
                new {id="Roscommon", Name="Roscommon"},
                new {id="Tipperary", Name="Tipperary"},
                new {id="Waterford", Name="Waterford"},
                new {id="Westmeath", Name="Westmeath"},
                new {id="Wexford", Name="Wexford"},
                new {id="Wicklow", Name="Wicklow"}
            },
        "ID", "Name", "Select County"
        );

        public override string ToString()
        {
            return String.Format("Address: {0}, Price: {1}, Date: {3}" ,Address, Price, DateOfSale);
        }
    }
}