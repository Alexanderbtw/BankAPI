using BankAPI.Data;
using BankAPI.Entities;
using BankAPI.Interfaces;
using BankAPI.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Principal;

namespace BankAPI.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRepository repository;
        public ValuesController(IRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public async Task<Client> GetClient([FromQuery] string? name = null)
        {
            //Через Header
            Request.Headers.TryGetValue("Username", out var headerUsernames);
            string? username = name ?? headerUsernames.FirstOrDefault();

            if (username != null)
            {
                Client? client = await repository.GetClientByUsernameAsync(username);
                if (client != null)
                    return client;
            }

            return new Client() { Id = 404, Username = "Error", Status = Data.Enums.Status.Individual, Accounts = new List<Account>() };
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<Client>> GetAll()
        {
            return await repository.GetAllClientsAsync();
        }

        [HttpPost]
        [Audit]
        public async Task AddClient([FromBody] Client client)
        {
             await repository.AddClientAsync(client);
        }

        [HttpPut]
        [Audit]
        public async Task UserUpdateInfo([FromBody] Client client)
        {
            var res = Task.Run(() => repository.UserUpdateInfo(client));
            if (await res == false)
            {
                Response.StatusCode = 400;
            }
        }

        [HttpPost]
        [Route("accounts")]
        [Audit]
        public async Task AddAccount([FromBody] Account account)
        {
            var res = await repository.AddAccountAsync(account);
            if (res == false)
            {
                Response.StatusCode = 400;
            }
        }

        [HttpPut]
        [Route("accounts")]
        [Audit]
        public async Task AccountTransaction(Account account)
        {
            var res = await repository.TransactionAsync(account.Id, account.Money);
            if (res == false)
            {
                Response.StatusCode = 400;
            }
        }

        [HttpDelete]
        [Route("accounts")]
        [Audit]
        public async Task<IResult> DeleteAccount(int accountId)
        {
            var res = await repository.DeleteAccountAsync(accountId);
            if (res == false)
            {
                return Results.NotFound();
            }
            return Results.Ok();
        }


        [HttpPut]
        [Route("transfers")]
        [Audit]
        public async Task AccountTransfer(AccountTransferData data)
        {
            var res = repository.TransferAsync(data);
            
            if (await res == false)
            {
                Response.StatusCode = 400;
            }
        }


        [HttpPut]
        [Route("transaction")]
        [Audit]
        public async Task GlobalTransaction(GlobalTransferData data)
        {
            var res = repository.GlobalTransferAsync(data);
            if (await res == false)
            {
                Response.StatusCode = 400;
            }
        }

        [HttpPost]
        [Route("contribution")]
        [Audit]
        public async Task CreateContribution(Contribution data)
        {
            var createTask = repository.CreateContributionAsync(data);
            if (await createTask == false)
            {
                Response.StatusCode = 400;
            }
        }
    }
}
