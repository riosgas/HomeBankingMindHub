using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using System.Collections.Generic;
using System;
using System.Linq;
using static HomeBankingMindHub.Services.ServiceExceptions;

namespace HomeBankingMindHub.Services
{
    public class ClientService : IClientService
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private readonly IAuthService _authService;

        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository, IAuthService authService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _authService = authService;
        }

        public List<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            var clientsDTO = new List<ClientDTO>();

            foreach (Client client in clients)
            {
                var newClientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Type = c.Type,
                        Color = c.Color,
                        Number = c.Number,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        ThruDate = c.ThruDate,
                    }).ToList(),
                };
                clientsDTO.Add(newClientDTO);
            }

            return clientsDTO;
        }

        public ClientDTO GetClientById(long id)
        {
            var client = _clientRepository.FindById(id);
            if (client == null)
                return null;
            ClientDTO clientDTO = new ClientDTO
            {
                Id = client.Id,
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Accounts = client.Accounts.Select(ac => new AccountDTO
                {
                    Id = ac.Id,
                    Balance = ac.Balance,
                    CreationDate = ac.CreationDate,
                    Number = ac.Number
                }).ToList(),
                Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                {
                    Id = cl.Id,
                    LoanId = cl.LoanId,
                    Name = cl.Loan.Name,
                    Amount = cl.Amount,
                    Payments = int.Parse(cl.Payments)
                }).ToList(),
                Cards = client.Cards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    CardHolder = c.CardHolder,
                    Type = c.Type,
                    Color = c.Color,
                    Number = c.Number,
                    Cvv = c.Cvv,
                    FromDate = c.FromDate,
                    ThruDate = c.ThruDate,
                }).ToList(),
            };

            return clientDTO;
        }

        public ClientDTO CreateClient(CreateClientRequestDTO client)
        {
            try
            {
                //validamos datos antes
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    throw new InvalidRequest();

                //buscamos si ya existe el usuario
                if (_clientRepository.FindByEmail(client.Email) != null)
                    throw new EmailAlreadyUsed();

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                _clientRepository.Save(newClient);
                Client createdClient = _clientRepository.FindByEmail(client.Email);

                ///pending check existing account numbers
                Random random = new Random();
                string randomAccountNumber = $"VIN-{random.Next(100000, 1000000).ToString()}";
                Account newAccount = new Account
                {
                    Number = randomAccountNumber,
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = createdClient.Id,
                };
                _accountRepository.Save(newAccount);

                ClientDTO createdClientDTO = GetClientById(createdClient.Id);
                return createdClientDTO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ClientDTO GetCurrentClient()
        {
            try
            {
                string UserAuthenticatedEmail = _authService.UserAuthenticated();
                if (UserAuthenticatedEmail == null) return null;

                Client client = _clientRepository.FindByEmail(UserAuthenticatedEmail);
                if (client == null) throw new ClientNotFoundException();

                ClientDTO clientDTO = GetClientById(client.Id);
                return clientDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
