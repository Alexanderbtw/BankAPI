using BankAPI.Data;
using BankAPI.Data.Enums;
using BankAPI.Database;
using BankAPI.Entities;
using BankAPI.Interfaces;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BankAPI.Tests
{
    [TestClass]
    public class IRepositoryTests
    {
        BankContext MemContext;
        static IHttpClientFactory HttpFactory;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient();

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            HttpFactory = mockFactory.Object;
        }

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: "BankContextInMemory")
                .Options;
            MemContext = new BankContext(options);
        }

        [TestCleanup]
        public void Cleanup()
        {
            MemContext.Dispose();
        }

        [TestMethod]
        public void AddClientAsync_ValidClient_TrueReturned()
        {
            // Arrange
            var testClient = new Client()
            {
                Id = 1,
                Accounts = new List<Account>(),
                Avatar = null,
                Contributions = new List<Contribution>(),
                Status = Status.Individual,
                Username = "username1"
            };

            var repo = new Repository(MemContext, new ConversionApi("test", HttpFactory));
            
            // Art
            bool actual = repo.AddClientAsync(testClient).Result;

            // Assert
            Assert.IsTrue(actual);
            Assert.IsNotNull(MemContext.Accounts);
        }

        [TestMethod]
        public void AddClientAsync_InvalidClient_FalseReturned()
        {
            // Arrange
            var testClient = new Client();
            var repo = new Repository(MemContext, new ConversionApi("test", HttpFactory));

            // Art
            var actual = repo.AddClientAsync(testClient).Result;

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void TransactionAsync_1000_TrueReturned()
        {
            // Arrange
            Account testAcc = new() { Currency = Currency.RUB, OwnerId = 1, Title = "TransactionTest" };
            MemContext.Accounts.Add(testAcc);
            MemContext.SaveChanges();
            var repo = new Repository(MemContext, new ConversionApi("test", HttpFactory));

            // Art
            var actual = repo.TransactionAsync(testAcc.Id, 1000).Result;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TransactionAsync_Minus1000_TrueReturned()
        {
            // Arrange
            Account testAcc = new() { Currency = Currency.RUB, OwnerId = 1, Money = 1000, Title = "TransactionTest" };
            MemContext.Accounts.Add(testAcc);
            MemContext.SaveChanges();
            var repo = new Repository(MemContext, new ConversionApi("test", HttpFactory));

            // Art
            var actual = repo.TransactionAsync(testAcc.Id, -1000).Result;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TransactionAsync_Minus2000_FalseReturned()
        {
            // Arrange
            Account testAcc = new() { Currency = Currency.RUB, OwnerId = 1, Money = 1000, Title = "TransactionTest" };
            MemContext.Accounts.Add(testAcc);
            MemContext.SaveChanges();
            var repo = new Repository(MemContext, new ConversionApi("test", HttpFactory));

            // Art
            var actual = repo.TransactionAsync(testAcc.Id, -2000).Result;

            // Assert
            Assert.IsFalse(actual);
        }
    }
}
