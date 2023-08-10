using HomeBankingMindHub.dtos;
using System.Collections.Generic;

namespace HomeBankingMindHub.Services
{
    public interface IAccountService
    {
        public List<AccountDTO> GetAllAccounts();
        public AccountDTO GetAccountById(long id);
        public AccountDTO CreateAccountForCurrentClient();
        public List<AccountDTO> GetCurrentClientAccounts();
    }
}
