using BankAPI.Data;
using BankAPI.Data.Enums;
using BankAPI.Database;
using BankAPI.Entities;
using BankAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BankAPI.Services
{
    public class Repository : IRepository
    {
        private BankContext bankContext;
        public IConversionService Converter { get; init; }

        public Repository(BankContext db, IConversionService converterService)
        {
            bankContext = db;
            Converter = converterService;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await bankContext.Clients.ToListAsync();
        }

        public async Task<bool> AddClientAsync(Client client)
        {
            var account = new Account()
            {
                Title = "Main",
                OwnerId = client.Id,
                Currency = Currency.RUB
            };
            client.Accounts.Add(account);
            try
            {
                await bankContext.Accounts.AddAsync(account);
                await bankContext.Clients.AddAsync(client);
                bankContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> AddAccountAsync(Account account)
        {
            try
            {
                await bankContext.Accounts.AddAsync(account);
                bankContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TransactionAsync(int accountId, decimal money)
        {
            // Cannot be translated (in Usit-Tests)
            //var numUpdated = await bankContext.Accounts.Where(a => a.Id == accountId && a.Money >= -money).ExecuteUpdateAsync(setters => setters.SetProperty(a => a.Money, a => a.Money + money));
            try
            {
                (await bankContext.Accounts.FirstAsync(a => a.Id == accountId && a.Money >= -money)).Money += money;
                bankContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<Client?> GetClientByUsernameAsync(string username)
        {
            var client = await bankContext.Clients.Include(c => c.Avatar).Include(c => c.Accounts).Include(c => c.Contributions).FirstOrDefaultAsync(x => x.Username == username);
            client!.Contributions.AsParallel().Where(c => c.EndDate <= DateOnly.FromDateTime(DateTime.Today)).ForAll(contribution =>
            {
                if (contribution.ParentAccount != null)
                {
                    contribution.ParentAccount.Money += contribution.Money;
                }
                else
                {
                    client.Accounts.OrderBy(a => a.Id).First().Money += Converter.Convert(contribution.Money, contribution.Currency, Currency.RUB);
                }
                bankContext.Contributions.Remove(contribution);
            });
            bankContext.Clients.Update(client);
            bankContext.SaveChanges();
            return client;
        }

        public bool UserUpdateInfo(Client client)
        {
            var newClient = bankContext.Clients.Update(client);
            if (newClient != null)
            {
                bankContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> GlobalTransferAsync(GlobalTransferData transferData)
        {
            var recipientAcc = (await bankContext.Accounts.FirstOrDefaultAsync(a => a.OwnerId == transferData.RecipientId && a.Title == "Main"))?.Id;
            if (recipientAcc != null)
                if (await TransferAsync(new AccountTransferData()
                        {
                            FromAccountId = transferData.AccountId,
                            FromCurrency = transferData.AccountCurrency,
                            Money = transferData.Money,
                            ToAccountId = recipientAcc ??= transferData.AccountId,
                            ToCurrency = Currency.RUB
                        }, transferData.Commission))
                { 
                    return true; 
                }
            return false;
        }

        public async Task<bool> TransferAsync(AccountTransferData data, float commission = 0)
        {
            if (!await TransactionAsync(data.FromAccountId, -data.Money)) return false;

            if (data.FromCurrency != data.ToCurrency)
            {
                data.Money = await Converter.ConvertAsync(data.Money, data.FromCurrency, data.ToCurrency);
            }

            if (!await TransactionAsync(data.ToAccountId, data.Money - data.Money / 100 * (decimal)commission)) return false;

            return true;
        }

        public async Task<bool> CreateContributionAsync(Contribution contribution)
        {
            if (contribution.ParentAccount == null)
                return false;

            try
            {
                contribution.ParentAccount.Money -= contribution.Money;
            }
            catch (Exception ex)
            {
                return false;
            }

            contribution.CalculeteProfit();

            (await bankContext.Clients.FirstAsync(c => c.Id == contribution.OwnerId)).Contributions.Add(contribution);
            bankContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = bankContext.Accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null) return false;

            var convertedMoney = await Converter.ConvertAsync(account.Money, account.Currency, Currency.RUB);
            await bankContext.Accounts.Where(a => a.OwnerId == account.OwnerId && a.Title == "Main").ExecuteUpdateAsync(setter => setter.SetProperty(a => a.Money, a => a.Money + convertedMoney));

            bankContext.Accounts.Entry(account).State = EntityState.Deleted;
            bankContext.SaveChanges();
            return true;
        }
    }
}
