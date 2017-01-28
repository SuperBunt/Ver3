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
using System.ComponentModel;

namespace AreaAnalyserVer3.ViewModels
{


    public class Analysis
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Analysis()
        {
            //Counties = new List<SelectListItem>();
            //Townlands = new List<SelectListItem>();
        }
        // fields
        private DateTime sixMonthsAgo;
        private List<PriceRegister> housesInArea;

        // Properties
        //public String Address { get; set; }
        public Town Town { get; set; }
        public GardaStation Garda { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<SelectListItem> Townlands { get; set; }
        public List<AnnualReport> Crimes { get; set; }
        public List<PriceRegister> HousesInArea { get; set; }
        public DateTime SixMonthsAgo { get; set; }
        [DisplayName("Avg. price")]
        public double AveragePriceLast6mths { get; set; }
        [DisplayName("% diff from national average")]
        public int PercentDiff
        {
            get
            {
                var national = db.PriceRegister.Where(y => y.DateOfSale > sixMonthsAgo).ToList();
                double nationalAvg = national.Average(p => (p.Price));
                double localAvg = HousesInArea.Average(p => (p.Price));

                double diff = nationalAvg - localAvg;

                return Convert.ToInt32((diff / localAvg) * 100);
            }
        }
        [DisplayName("Units sold")]
        public int NumSoldinLast6mths
        {
            get
            {
                return HousesInArea.Count();
            }
        }
        #region Methods

        #endregion

    }


}