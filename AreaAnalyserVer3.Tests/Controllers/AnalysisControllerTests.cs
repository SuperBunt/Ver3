using Microsoft.VisualStudio.TestTools.UnitTesting;
using AreaAnalyserVer3.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AreaAnalyserVer3.ViewModels;
using AreaAnalyserVer3.Models;
using Google.Apis.Util;
using System.Reflection;
using System.Web.Mvc;

namespace AreaAnalyserVer3.Controllers.Tests
{
    [TestClass()]
    public class AnalysisControllerTests
    {
        ApplicationDbContext db = new ApplicationDbContext();



        [TestMethod()]
        public void IndexTest()
        {

        }

        [TestMethod()]
        public void GetHousesTest()
        {
            // Arrange
            string name = "skerries";
            AnalysisController test_controller = new AnalysisController();

            //Act
            JsonResult result = test_controller.GetHouses(name);
            // Act
            Assert.IsNotNull(result, "No ActionResult returned from action method.");
            dynamic jsonObject = result.Data;
            Assert.IsTrue(jsonObject.Success);
            //Assert.That(result.Data, Is.Not.Null, "There should be some data for the JsonResult");
            //Assert.That(result.Data.GetReflectedProperty("page"), Is.EqualTo(page));
            //Assert.Fail();
        }

        // In order to populate the property graph the units need to be grouped and
        // averaged to a specific format
        [TestMethod()]
        public void ValidateChartDataTest()
        {
            // Arrange          
            List<PriceRegister> sampleHouses = new List<PriceRegister>();
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 1, 1), Price = 10000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 1, 1), Price = 10000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 1, 1), Price = 10000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 2, 1), Price = 200000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 2, 1), Price = 200000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 2, 1), Price = 200000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 3, 1), Price = 30000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 3, 1), Price = 30000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 3, 1), Price = 30000 });
            sampleHouses.Add(new PriceRegister() { DateOfSale = new DateTime(2016, 3, 1), Price = 30000 });
            // Act
            var avg = (sampleHouses.OrderBy(x => x.DateOfSale)
              .GroupBy(x => new { x.DateOfSale.Year, x.DateOfSale.Month })
              .Select(p => new
              {
                  date_sold = string.Format("{0},{1}", p.Key.Year, p.Key.Month),
                  avg_price = p.Average(i => i.Price)
              })).ToList();
            //Assert
            // The required result = 
            //  [
            //  {
            //       "date_sold": "2016,01",
            //	     "avg_price": 10000
            //      },
            //  {
            //      "date_sold": "2016,02",
            //	    "avg_price": 200000
            //   } ]

            Assert.AreSame(avg[0].date_sold, "2016,01");
            Assert.AreSame(avg[0].avg_price, 10000);
            Assert.AreSame(avg[1].date_sold, "2016,02");
            Assert.AreSame(avg[1].avg_price, 200000);
            Assert.AreSame(avg[2].date_sold, "2016,03");
            Assert.AreSame(avg[2].avg_price, 30000);
            Assert.Fail();
        }

        [TestMethod()]
        public void GetBusinessesTest()
        {
            // Arrange
            const int ID = 150;
            AnalysisController test_controller = new AnalysisController();

            //Act
            JsonResult result = test_controller.GetBusinesses(ID);
            dynamic jsonObject = result.Data;

            // Assert
            Assert.IsNotNull(result, "No ActionResult returned from action method.");
            Assert.IsTrue(jsonObject.Success);
        }

        // helper method
        //public static object GetReflectedProperty(this object obj, string propertyName)
        //{
        //    obj.ThrowIfNull("obj");
        //    propertyName.ThrowIfNull("propertyName");

        //    PropertyInfo property = obj.GetType().GetProperty(propertyName);

        //    if (property == null)
        //    {
        //        return null;
        //    }

        //    return property.GetValue(obj, null);
        //}
    }
}