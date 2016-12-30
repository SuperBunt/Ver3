using AreaAnalyserVer3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Data.Entity.Spatial;
using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace AreaAnalyserVer3.ViewModels
{
    

    public class Analysis
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Analysis() {
            //Counties = new List<SelectListItem>();
            //Townlands = new List<SelectListItem>();
        }
        // Properties
        public String Address { get; set; }
        public Town Town { get; set; }
        public GardaStation Garda { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<SelectListItem> Townlands { get; set; }
        public List<AnnualReport> Crimes { get; set; }
        public List<PriceRegister> HousesInArea {
            get
            {
                List<PriceRegister> houses = new List<PriceRegister>();
                var query = from p in db.PriceRegister
                            select p;
                var suggestions = (from a in query
                   where Town.LocalSpellings.Any(word => a.Address.Contains(word))
                   select a);
                
                return houses = suggestions.OrderByDescending(x => x.DateOfSale).ToList(); ;
            }
        }

        #region Methods
        
        #endregion

    }


}