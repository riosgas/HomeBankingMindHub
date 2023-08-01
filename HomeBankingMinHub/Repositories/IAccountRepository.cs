using HomeBankingMindHub.Models;
using System.Collections.Generic;

namespace HomeBankingMindHub.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
        Account FindByNumber(string number);
        void Save(Account account);
        IEnumerable<Account> GetAccountsByClient(long clientId);
    }
}
