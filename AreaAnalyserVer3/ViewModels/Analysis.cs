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
using System.Threading.Tasks;
using System.Data.Entity;

namespace AreaAnalyserVer3.ViewModels
{


    public class Analysis
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Analysis(string TownID)
        {
            sixMonthsAgo = DateTime.Today.AddMonths(-6);
            AreaName = TownID;
            //housesInArea = GetLocalHouses(TownID);
            // averagePriceLast6mths 
        }
        // fields
        private DateTime sixMonthsAgo;
        private List<PriceRegister> housesInArea;
        private double averagePriceLast6mths;

        // Properties
        //public String Address { get; set; }
        public Town Town { get; set; }
        public string AreaName { get; set; }
        public GardaStation Garda { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<SelectListItem> Townlands { get; set; }
        public List<AnnualReport> Crimes { get; set; }
        public List<PriceRegister> HousesInArea {
            get {
                //var county = db.PriceRegister.Where(x => x.t.Equals(Town.County));
                //  Town.LocalSpellings is used to allow for irish spellings of the town         
                //var query = (from a in county
                //             where Town.LocalSpellings.Any(word => a.Address.Contains(word))
                //             select a);
                var query = db.PriceRegister.Where(x => x.Address.Contains(AreaName));

                return query.ToList();
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
                if (NumSoldinLast6mths > 0)
                    return 120000;
                //return averagePriceLast6mths = housesInArea.Where(y => y.DateOfSale > sixMonthsAgo).Average(p => p.Price);
                else
                    return 0;
            }
      }
        [DisplayName("% diff from national average")]
        public int PercentDiff
        {
            get
            {
                return 10;
                //var national = db.PriceRegister.Where(y => y.DateOfSale > sixMonthsAgo);
                //double nationalAvg = national.Average(p => (p.Price));
                //// double localAvg = HousesInArea.Where(y => y.DateOfSale > sixMonthsAgo).Average(p => (p.Price));
                //// If no house sold in last 6 months then the average is 0
                //if (national.Count() == 0)
                //{
                //    return 0;
                //}
                //double diff = nationalAvg - averagePriceLast6mths;

                //return Convert.ToInt32((diff / averagePriceLast6mths) * 100);
            }
        }
        [DisplayName("Units sold over last 6 months")]
        public int NumSoldinLast6mths
        {
            get
            {
                return 100;
                //return housesInArea.Where(y => y.DateOfSale > sixMonthsAgo).Count();
            }
        }
        #region Methods

        #endregion
        //public List<PriceRegister> GetLocalHouses(string id)
        //{
        //    var Town = db.Town.Find(Int32.Parse(id));
        //    var county = db.PriceRegister.Where(x => x.County.Equals(Town.County));
        //    //  Town.LocalSpellings is used to allow for irish spellings of the town         
        //    var query = (from a in county
        //                 where Town.LocalSpellings.Any(word => a.Address.Contains(word))
        //                 select a);

        //    return query.ToList();
        //}

    }


}