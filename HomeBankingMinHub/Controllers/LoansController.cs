using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        private ILoanRepository _loanRepository;
        private IClientLoanRepository _clientLoanRepository;
        private ITransactionRepository _transactionRepository;
        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository,
            ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;

        }

        [HttpGet()]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAll();
                var loansDTO = new List<LoanDTO>();

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

                return Ok(loansDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost()]
        public IActionResult CreateLoanToCurrent([FromBody] LoanApplicationDTO loanApplication)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) return Forbid("Usuario no autenticado");
                Client currentClient = _clientRepository.FindByEmail(email);
                if (currentClient == null) return Forbid();

                //check loan
                //if (loanApplication.LoanId <= 0) return Forbid("Id de préstamo no válido");
                var loan = _loanRepository.FindById(loanApplication.LoanId);
                if (loan == null) return StatusCode(403, "El préstamo solicitado no existe");

                //check amount
                if (loanApplication.Amount < 1 || loanApplication.Amount > loan.MaxAmount)
                    return StatusCode(403, $"El monto debe estar entre 1 y {loan.MaxAmount}");

                //check payments
                List<int> paymentOptions = loan.Payments.Split(',').Select(int.Parse).ToList();
                if (!paymentOptions.Contains(loanApplication.Payments))
                    return StatusCode(403, $"Las cuotas disponibles para el préstamo {loan.Name} son {loan.Payments}");

                //check accounts
                var account = _accountRepository.FindByNumber(loanApplication.ToAccountNumber);
                if (account == null) return Forbid("La cuenta ingresada no existe");
                if (account.ClientId != currentClient.Id)
                    return StatusCode(403, "La cuenta no pertenece al cliente autenticado");
                
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

                return Created("Préstamo creado", newClientLoan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
