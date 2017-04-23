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
using System.ComponentModel.DataAnnotations;

namespace AreaAnalyserVer3.ViewModels
{


    public class Analysis
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Analysis()
        {
            housesInArea = new List<PriceRegister>();
        }

        public Analysis(string TownID)
        {
        }
        // fields

        private List<PriceRegister> housesInArea;
        internal DateTime sixMonthsAgo;

        // private double averagePriceLast6mths;
        //private string percentDiff;

        // Properties
        public Town Town { get; set; }
        [DisplayName("Businesses/services")]
        public int NumBusinesses { get; set; }
        [DisplayName("Schools")]
        public int NumSchools { get; set; }
        [DisplayName("Area")]
        public string AreaName { get; set; }
        public string NearestStation { get; set; }
        public GardaStation Garda { get; set; }

        DateTime Today { get; set; } = DateTime.Now;
        DateTime SixMonthsAgo { get; set; } = DateTime.Now.AddMonths(-6);

        public int sixMonthsAgoInt { get; set; } = DateTime.Now.AddMonths(-6).Month;
        public int YearsixMonthsAgo { get; set; } = DateTime.Now.AddMonths(-6).Year;
        public int nowMonth { get; set; } = DateTime.Now.Month;
        public int nowYear { get; set; } = DateTime.Now.Year;

        public List<Business> Businesses { get; set; }
        public List<AnnualReport> Crimes { get; set; }
        public List<School> Schools { get; set; }
        public List<PriceRegister> HousesInArea { get; set; }
        public List<PriceRegister> LastSixMonths { get; set; }
        public List<WikiContent> WikiResults { get; set; }
        [DisplayName("Units sold ")]
        public int NumSoldinLast6mths { get; set; }

        [DisplayName("Avg. price")]
        [DisplayFormat(DataFormatString = "€ {0:0}")]
        public double AveragePriceLast6mths { get; set; }

        [DisplayName("% increase")]
        public string PercentDiff
        {
            get
            {
                if (NumSoldinLast6mths > 0)
                {

                    try
                    {
                        var previousAvg = LastSixMonths.Where(y => y.DateOfSale.Month == sixMonthsAgoInt && y.DateOfSale.Year == YearsixMonthsAgo).Average(p => p.Price);
                        // we'll use to accumalated average as default
                        var currentAvg = AveragePriceLast6mths;
                        // The actual value we desire is the average price this for six months ago
                        if (LastSixMonths.Where(y => y.DateOfSale.Month == nowMonth && y.DateOfSale.Year == nowYear).Count() == 0)
                            currentAvg = LastSixMonths.Where(y => y.DateOfSale.Month == nowMonth && y.DateOfSale.Year == nowYear).Average(p => p.Price);
                       

                        // work out the difference (increase) between the two numbers you are comparing.
                        // Ensure there was houses sold in the last 6 months

                        double increase = Math.Round(currentAvg) - Math.Round(previousAvg);

                        double percent = (increase / previousAvg) * 100;

                        return String.Format("{0:0.0}", percent);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return "Sorry we dont have that information!";
                    }
                }
                return "Cannot compare, No houses sold in previous 6 months.";

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