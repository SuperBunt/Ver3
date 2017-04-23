using Microsoft.VisualStudio.TestTools.UnitTesting;
using AreaAnalyserVer3.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AreaAnalyserVer3.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public async void ContactTest()
        {
            // Arrange
            HomeController controller = new HomeController();
            Models.Email email = new Models.Email();
            email.FromName = "Peter";
            email.FromEmail = "tester@outlook.ie";
            email.Subject = "This is a test";
            email.Message = "Another message";
            // Act
            await controller.Contact(email);
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            


            throw new NotImplementedException();
        }
    }
}