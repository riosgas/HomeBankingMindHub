using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Enums;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static HomeBankingMindHub.Services.ServiceExceptions;

namespace HomeBankingMindHub.Services
{
    public class LoanService : ILoanService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAuthService _authService;

        public LoanService(IClientRepository clientRepository, ILoanRepository loanRepository,
            IClientLoanRepository clientLoanRepository, IAccountRepository accountRepository,
            ITransactionRepository transactionRepository, IAuthService authService)
        {
            _clientRepository = clientRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _authService = authService;
        }

        public List<LoanDTO> GetAllLoans()
        {
            var loans = _loanRepository.GetAll();
            List<LoanDTO> loansDTO = new List<LoanDTO>();

            foreach (Loan loan in loans)
            {
                var newLoanDTO = new LoanDTO
                {
                    Id = loan.Id,
                    Name = loan.Name,
                    MaxAmount = loan.MaxAmount,
                    Payments = loan.Payments,
                };
                loansDTO.Add(newLoanDTO);
            }
            return loansDTO;
        }
        public ClientLoanDTO CreateClientLoanForCurrent(LoanApplicationDTO loanApplication)
        {
            string UserAuthenticatedEmail = _authService.UserAuthenticated();
            if (UserAuthenticatedEmail == null) return null;

            Client currentClient = _clientRepository.FindByEmail(UserAuthenticatedEmail);
            if (currentClient == null) throw new ClientNotFoundException();

            //check loan
            //if (loanApplication.LoanId <= 0) return Forbid("Id de préstamo no válido");
            var loan = _loanRepository.FindById(loanApplication.LoanId);
            if (loan == null) throw new Exception("El préstamo solicitado no existe");

            //check amount
            if (loanApplication.Amount < 1 || loanApplication.Amount > loan.MaxAmount)
                throw new Exception($"El monto debe estar entre 1 y {loan.MaxAmount}");

            //check payments
            List<int> paymentOptions = loan.Payments.Split(',').Select(int.Parse).ToList();
            if (!paymentOptions.Contains(loanApplication.Payments))
                throw new Exception($"Las cuotas disponibles para el préstamo {loan.Name} son {loan.Payments}");

            //check accounts
            var account = _accountRepository.FindByNumber(loanApplication.ToAccountNumber);
            if (account == null)
                throw new Exception("La cuenta ingresada no existe");
            if (account.ClientId != currentClient.Id)
                throw new Exception("La cuenta no pertenece al cliente autenticado");

            //create ClientLoan
            ClientLoan newClientLoan = new ClientLoan
            {
                Amount = loanApplication.Amount * 1.2,
                Payments = loanApplication.Payments.ToString(),
                ClientId = currentClient.Id,
                LoanId = loanApplication.LoanId,
            };
            _clientLoanRepository.Save(newClientLoan);

            //create deposit transaction
            Transaction newLoanDeposit = new Transaction
            {
                Type = TransactionType.CREDIT.ToString(),
                Amount = loanApplication.Amount,
                Description = $"{loan.Name} loan approved",
                AccountId = account.Id,
                Date = DateTime.Now,
            };
            _transactionRepository.Save(newLoanDeposit);

            //update balance
            account.Balance = account.Balance + loanApplication.Amount;
            _accountRepository.Save(account);

            ClientLoanDTO newClientLoanDTO = new ClientLoanDTO
            {
                LoanId = newClientLoan.LoanId,
                Name = _loanRepository.FindById(newClientLoan.LoanId).Name,
                Amount = newClientLoan.Amount,
                Payments = int.Parse(newClientLoan.Payments),
            };
            return newClientLoanDTO;
        }
    }
}
