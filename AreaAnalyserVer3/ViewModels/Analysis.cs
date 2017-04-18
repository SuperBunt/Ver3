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
        private double averagePriceLast6mths;

        // Properties
        public Town Town { get; set; }
        [DisplayName("No. of listed businesses")]
        public int NumBusinesses { get; set; }
        [DisplayName("No. schools")]
        public int NumSchools { get; set; }
        [DisplayName("Area")]
        public string AreaName { get; set; }
        public string NearestStation { get; set; }
        public GardaStation Garda { get; set; }

        public List<Business> Businesses { get; set; }
        public List<AnnualReport> Crimes { get; set; }
        public List<School> Schools { get; set; }
        public List<PriceRegister> HousesInArea { get; set; }
        public List<WikiContent> WikiResults { get; set; }
        //public IEnumerable<WikiImage> WikiImages { get; set; }
        [DisplayName("* Units sold ")]
        public int NumSoldinLast6mths { get; set; }

        [DisplayName("* Avg. price")]
        [DisplayFormat(DataFormatString = "{0:C0, en-IE}")]
        public double AveragePriceLast6mths { get; set; }

        [DisplayName("* % diff")]
        public string PercentDiff
        {
            get
            {
                var sixAgo = DateTime.Today.AddMonths(-6);
                var national = db.PriceRegister.Where(y => y.DateOfSale > sixAgo);

                // The values are being truncated because too many decimals may result in infinity
                // being returned later in the calculation
                double nationalAvg = Math.Truncate(national.Average(p => (p.Price)));
                // If no house sold in last 6 months then the average is 0
                if (national.Count() == 0)
                {
                    return "0";
                }
                // work out the difference (increase) between the two numbers you are comparing.
                double increase = nationalAvg - Math.Truncate(AveragePriceLast6mths);

                double percent = (increase / AveragePriceLast6mths) * 100;

                return String.Format("{0:0.#}", percent);

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