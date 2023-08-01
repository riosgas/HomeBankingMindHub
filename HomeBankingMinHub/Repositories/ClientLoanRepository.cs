using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace HomeBankingMindHub.Repositories
{
    public class ClientLoanRepository : RepositoryBase<ClientLoan>, IClientLoanRepository
    {
        public ClientLoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext) {}
        public void Save(ClientLoan clientLoan)
        {
            Create(clientLoan);
            SaveChanges();
        }
    }
}

