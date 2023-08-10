using HomeBankingMindHub.dtos;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Services
{
    public interface ITransactionService
    {
        public AccountDTO ProcessTransaction(TransferDTO transferDTO);
    }
}
