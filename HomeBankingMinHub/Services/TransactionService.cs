using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Enums;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeBankingMindHub.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;

        public TransactionService(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository, IAuthService authService, IAccountService accountService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _authService = authService;
            _accountService = accountService;
        }

        public AccountDTO ProcessTransaction(TransferDTO transferDTO)
        {
            string UserAuthenticatedEmail = _authService.UserAuthenticated();
            if (UserAuthenticatedEmail == null) return null;

            Client currentClient = _clientRepository.FindByEmail(UserAuthenticatedEmail);
            if (currentClient == null) throw new Exception("Cliente no encontrado");

            if (transferDTO.FromAccountNumber == string.Empty || transferDTO.ToAccountNumber == string.Empty)
                throw new Exception("Cuenta de origen o cuenta de destino no proporcionada.");

            if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
                throw new Exception("No se permite la transferencia a la misma cuenta.");

            if (transferDTO.Amount == 0 || transferDTO.Description == string.Empty)
                throw new Exception("Monto o descripción no proporcionados.");

            //buscamos las cuentas
            Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
            if (fromAccount == null)
                throw new Exception("Cuenta de origen no existe");

            //controlamos el monto
            if (fromAccount.Balance < transferDTO.Amount)
                throw new Exception("Fondos insuficientes");

            //buscamos la cuenta de destino
            Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
            if (toAccount == null)
                throw new Exception("Cuenta de destino no existe");

            //a la cuenta de origen le restamos el monto
            //a la cuenta de destino le sumamos el monto
            fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
            toAccount.Balance = toAccount.Balance + transferDTO.Amount;
            _accountRepository.Save(fromAccount);
            _accountRepository.Save(toAccount);

            //demas logica para guardado
            //comenzamos con la inserción de las 2 transacciones realizadas
            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.DEBIT.ToString(),
                Amount = transferDTO.Amount * -1,
                Description = $"{transferDTO.Description} {toAccount.Number}",
                AccountId = fromAccount.Id,
                Date = DateTime.Now,
            });

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.CREDIT.ToString(),
                Amount = transferDTO.Amount,
                Description = $"{transferDTO.Description} {fromAccount.Number}",
                AccountId = toAccount.Id,
                Date = DateTime.Now,
            });

            AccountDTO fromAccountDTO = _accountService.GetAccountById(fromAccount.Id);
            return fromAccountDTO;
        }
    }
}
