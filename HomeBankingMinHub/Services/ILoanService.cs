using HomeBankingMindHub.dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HomeBankingMindHub.Services
{
    public interface ILoanService
    {
        public List<LoanDTO> GetAllLoans();
        public ClientLoanDTO CreateClientLoanForCurrent(LoanApplicationDTO loanApplication);
    }
}
