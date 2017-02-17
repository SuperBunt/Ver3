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

        public Analysis(string TownID)
        {
            sixMonthsAgo = DateTime.Today.AddMonths(-6);
            housesInArea = GetLocalHouses(TownID);
            averagePriceLast6mths = housesInArea.Where(y => y.DateOfSale > sixMonthsAgo).Average(p => p.Price);
        }
        // fields
        private DateTime sixMonthsAgo;
        private List<PriceRegister> housesInArea;
        private double averagePriceLast6mths;

        // Properties
        //public String Address { get; set; }
        public Town Town { get; set; }
        public GardaStation Garda { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<SelectListItem> Townlands { get; set; }
        public List<AnnualReport> Crimes { get; set; }
        public List<PriceRegister> HousesInArea
        {
            get
            {
                return housesInArea;
            }
            set
            {
                housesInArea = value;
            }
        }
        public List<WikiContent> WikiResults { get; set; }
        //public IEnumerable<WikiImage> WikiImages { get; set; }
        public DateTime SixMonthsAgo { get; set; }
        [DisplayName("Avg. price")]
        public double AveragePriceLast6mths 
        {
            get
            {
                return averagePriceLast6mths;
            }
            set
            {
                averagePriceLast6mths = value;
            }
        }
        [DisplayName("% diff from national average")]
        public int PercentDiff
        {
            get
            {
                var national = db.PriceRegister.Where(y => y.DateOfSale > sixMonthsAgo);
                double nationalAvg = national.Average(p => (p.Price));
                // double localAvg = HousesInArea.Where(y => y.DateOfSale > sixMonthsAgo).Average(p => (p.Price));

                double diff = nationalAvg - averagePriceLast6mths;

                return Convert.ToInt32((diff / averagePriceLast6mths) * 100);
            }
        }
        [DisplayName("Units sold over last 6 months")]
        public int NumSoldinLast6mths
        {
            get
            {
                int count;
                return count = housesInArea.Where(y => y.DateOfSale > sixMonthsAgo).Count();
            }
        }
        #region Methods

        #endregion
        public List<PriceRegister> GetLocalHouses(string id)
        {
            var Town = db.Town.Find(Int32.Parse(id));
            //  Town.LocalSpellings is used to allow for irish spellings of the town         
            var query = (from a in db.PriceRegister
                                where Town.LocalSpellings.Any(word => a.Address.Contains(word) && a.County.Equals(Town.County))
                                select a).OrderByDescending(x => x.DateOfSale).ToList();
            
            return query;
        }

    }


}