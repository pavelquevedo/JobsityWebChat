using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using WebChat.Client.Controllers;
using WebChat.Utils.Common.Models.Request;

namespace WebChat.Client.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void LoginIndex()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Login() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ViewBag.Title);
        }

        [TestMethod]
        public void LoginSuccess()
        {
            // Arrange
            HomeController controller = new HomeController();
            
            //Setup mock
            var mockLoginRequest = new Mock<LoginRequest>();
            mockLoginRequest.Setup(m => m.Login).Returns("pavel");
            mockLoginRequest.Setup(m => m.Password).Returns("03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4");

            // Act
            ViewResult result = controller.Login(mockLoginRequest.Object) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Lobby", result.ViewBag.Title);
        }

    }
}
