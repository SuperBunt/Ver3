using Microsoft.VisualStudio.TestTools.UnitTesting;
using AreaAnalyserVer3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AreaAnalyserVer3.Models;

namespace AreaAnalyserVer3.ViewModels.Tests
{
    [TestClass()]
    public class AnalysisTests
    {
        [TestMethod()]
        public void AnalysisTest()
        {
            // Arrange          
            List<PriceRegister> sampleHouses = new List<PriceRegister>();
            Analysis Tester = new Analysis();
            // Add house from 6 months ago
            DateTime now = DateTime.Now;
            DateTime sixMnthsAgo = now.AddMonths(-6);
            DateTime threeMonthsAgo = now.AddMonths(-3);

            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime (2015,1,1), Price = 100000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2015, 1, 1), Price = 100000 });

            sampleHouses.Add(new PriceRegister() { DateOfSale = sixMnthsAgo, Price = 100000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = sixMnthsAgo, Price = 100000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = sixMnthsAgo, Price = 100000 });

            sampleHouses.Add(new PriceRegister() { DateOfSale = now, Price = 120000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = now, Price = 120000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = now, Price = 120000 });
           
            Tester.HousesInArea = sampleHouses;

            Tester.NumSoldinLast6mths = Tester.HousesInArea.Where(y => y.DateOfSale >= sixMnthsAgo).Count();
            Tester.LastSixMonths = Tester.HousesInArea.Where(y => y.DateOfSale >= sixMnthsAgo).ToList();
            Assert.AreEqual(6, Tester.NumSoldinLast6mths);
            Assert.AreEqual("20", Tester.PercentDiff);
        }

        [TestMethod()]
        public void AveragePriceTest()
        {
            // Arrange          
            List<PriceRegister> sampleHouses = new List<PriceRegister>();
            Analysis Tester = new Analysis();
            // Act
            // Add house from 6 months ago
            DateTime now = DateTime.Now;
            DateTime sixMnthsAgo = now.AddMonths(-6);
            DateTime threeMonthsAgo = now.AddMonths(-3);

            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2015, 1, 1), Price = 100000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2015, 1, 1), Price = 100000 });

            sampleHouses.Add(new PriceRegister() { DateOfSale = sixMnthsAgo, Price = 100000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = sixMnthsAgo, Price = 100000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = sixMnthsAgo, Price = 100000 });

            sampleHouses.Add(new PriceRegister() { DateOfSale = now, Price = 120000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = now, Price = 120000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = now, Price = 120000 });

            Tester.HousesInArea = sampleHouses;

            Tester.NumSoldinLast6mths = Tester.HousesInArea.Where(y => y.DateOfSale >= sixMnthsAgo).Count();
            Tester.LastSixMonths = Tester.HousesInArea.Where(y => y.DateOfSale >= sixMnthsAgo).ToList();
            Tester.AveragePriceLast6mths = Tester.LastSixMonths.Average(p => p.Price);
            // Assert
            Assert.AreEqual(110000, Tester.AveragePriceLast6mths);
        }
    }
}