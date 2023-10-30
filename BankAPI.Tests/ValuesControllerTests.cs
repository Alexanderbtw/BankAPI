using BankAPI.Controllers;
using BankAPI.Database;
using BankAPI.Entities;
using BankAPI.Interfaces;
using BankAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics;

namespace BankAPI.Tests
{
    [TestClass]
    public class ValuesControllerTests
    {
        //public required TestContext TestContext { get; set; }

        [TestMethod]
        public void GetClient_Username1_ReturnsValidClient()
        {
            //Arrange
            string username = "username1";
            var repoMock = new Mock<IRepository> { CallBase = true };
            var testClient = new Client()
            {
                Id = 1,
                Accounts = new List<Account>(),
                Avatar = null,
                Contributions = new List<Contribution>(),
                Status = Data.Enums.Status.Individual,
                Username = "username1"
            };
            repoMock.Setup(obj => obj.GetClientByUsernameAsync("username1"))
                .ReturnsAsync(testClient)
                .Verifiable();
            ValuesController controller = new( repoMock.Object );
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();


            //Art
                  // With Headers
            controller.Request.Headers.Add("Username", username);
            var actualHeader = controller.GetClient();
            controller.Request.Headers.Remove("Username");

                  // With Query
            var actualQuery = controller.GetClient(username);

             


            //Assert
            Assert.AreEqual(username, actualQuery.Result.Username);
            Assert.AreEqual(username, actualHeader.Result.Username);
        }
    }
}