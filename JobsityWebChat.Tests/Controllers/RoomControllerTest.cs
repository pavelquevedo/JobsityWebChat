using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebChat.Api.Controllers;
using WebChat.Utils.Common.Models.Response;

namespace JobsityWebChat.Tests.Controllers
{
    [TestClass]
    public class RoomControllerTest
    {
        [TestMethod]
        public void GetAll()
        {
            // Arrange
            RoomController controller = new RoomController();

            Task<IHttpActionResult> httpActionResult = controller.GetAll();
            // Act
            var contentResult = httpActionResult.Result as OkNegotiatedContentResult<List<RoomResponse>>;

            // Assert
            Assert.IsNotNull(httpActionResult.Result);

            //Checking users data
            Assert.IsInstanceOfType(contentResult.Content, typeof(List<RoomResponse>));

            //Check if statuscode = 200OK
            Assert.IsInstanceOfType(httpActionResult.Result, typeof(OkNegotiatedContentResult<List<RoomResponse>>));

        }
    }
}
