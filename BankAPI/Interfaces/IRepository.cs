using BankAPI.Data;
using BankAPI.Entities;

namespace BankAPI.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<bool> AddClientAsync(Client client);
        Task<Client?> GetClientByUsernameAsync(string username);
        Task<bool> AddAccountAsync(Account account);
        Task<bool> TransactionAsync(int accountId, decimal money);
        Task<bool> GlobalTransferAsync(GlobalTransferData transferData);
        bool UserUpdateInfo(Client client);
        Task<bool> TransferAsync(AccountTransferData data, float commission = 0);
        Task<bool> CreateContributionAsync(Contribution contribution);
        Task<bool> DeleteAccountAsync(int accountId);
    }
}
