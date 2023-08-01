using HomeBankingMindHub.Models;
using System.Collections.Generic;

namespace HomeBankingMindHub.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAll();
        Loan FindById(long id);
    }
}
