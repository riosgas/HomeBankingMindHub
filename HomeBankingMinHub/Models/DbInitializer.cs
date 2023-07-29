using HomeBankingMindHub.Models;
using System;
using System.Linq;

namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            #region Clients data
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { FirstName="Gaston", LastName="Alvaro", Email = "ga@gmail.com", Password="123456"},
                    new Client { FirstName="Juan", LastName="Carlos", Email = "jc@gmail.com", Password="123456"}
                };
                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }
                context.SaveChanges();
            }
            #endregion

            #region Accounts data
            if (!context.Accounts.Any())
            {
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "ga@gmail.com");
                if (client1 != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = client1.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 100 },
                        new Account {ClientId = client1.Id, CreationDate = DateTime.Now, Number = "VIN002", Balance = 0 },
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "jc@gmail.com");
                if (client2 != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = client2.Id, CreationDate = DateTime.Now, Number = "VIN003", Balance = 100 },
                        new Account {ClientId = client2.Id, CreationDate = DateTime.Now, Number = "VIN004", Balance = 0 },
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();
            }
            #endregion

            #region Transactions data
            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia recibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda Mercado Libre", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda Random", Type = TransactionType.DEBIT.ToString() },
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                }
                var account3 = context.Accounts.FirstOrDefault(c => c.Number == "VIN003");
                if (account3 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account3.Id, Amount = 5000, Date= DateTime.Now.AddHours(-2), Description = "Transferencia recibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account3.Id, Amount = -2500, Date= DateTime.Now.AddHours(-1), Description = "Compra en PedidosYa", Type = TransactionType.DEBIT.ToString() },
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                }
                context.SaveChanges();
            }
            #endregion

            #region Loans data
            if (!context.ClientLoans.Any())
            {
                //add loans types
                if (!context.Loans.Any())
                {
                    var loans = new Loan[]
                    {
                        new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                        new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                        new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                    };
                    foreach (Loan loan in loans)
                    {
                        context.Loans.Add(loan);
                    };
                    context.SaveChanges();
                }
                var loanHipoteca = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                var loanPersonal = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                var loanAuto = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");

                //add clientloans
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "ga@gmail.com");
                if (client1 != null)
                {
                    var clientLoans = new ClientLoan[]
                    {
                        new ClientLoan { Amount = 400000, ClientId = client1.Id, LoanId = loanHipoteca.Id, Payments = "60" },
                        new ClientLoan { Amount = 100000, ClientId = client1.Id, LoanId = loanPersonal.Id, Payments = "24" },
                        new ClientLoan { Amount = 300000, ClientId = client1.Id, LoanId = loanAuto.Id, Payments = "36" },
                    };
                    foreach (ClientLoan clientLoan in clientLoans)
                    {
                        context.ClientLoans.Add(clientLoan);
                    }
                }

                var client2 = context.Clients.FirstOrDefault(c => c.Email == "jc@gmail.com");
                if (client2 != null && client2?.ClientLoans == null)
                {
                    var clientLoans = new ClientLoan[]
                    {
                        new ClientLoan { Amount = 100000, ClientId = client2.Id, LoanId = loanPersonal.Id, Payments = "24" },
                        new ClientLoan { Amount = 300000, ClientId = client2.Id, LoanId = loanAuto.Id, Payments = "36" },
                    };
                    foreach (ClientLoan clientLoan in clientLoans)
                    {
                        context.ClientLoans.Add(clientLoan);
                    }
                }
                context.SaveChanges();
            }
            #endregion

            #region Cards data
            if (!context.Cards.Any())
            {
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "ga@gmail.com");
                if (client1 != null)
                {
                    var cards = new Card[]
                    {
                        new Card {
                            ClientId= client1.Id, CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.DEBIT.ToString(), Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7876-4445", Cvv = 990,
                            FromDate= DateTime.Now, ThruDate= DateTime.Now.AddYears(4),
                        },
                        new Card {
                            ClientId= client1.Id, CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.CREDIT.ToString(), Color = CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-7888", Cvv = 750,
                            FromDate= DateTime.Now, ThruDate= DateTime.Now.AddYears(5),
                        },
                    };
                    foreach (Card card in cards)
                    {
                        context.Cards.Add(card);
                    }
                }
                
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "jc@gmail.com");
                if (client2 != null)
                {
                    var cards = new Card[]
                    {
                        new Card {
                            ClientId= client2.Id, CardHolder = client2.FirstName + " " + client2.LastName,
                            Type = CardType.DEBIT.ToString(), Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7777-4444", Cvv = 330,
                            FromDate= DateTime.Now, ThruDate= DateTime.Now.AddYears(6),
                        },
                    };
                    foreach (Card card in cards)
                    {
                        context.Cards.Add(card);
                    }
                }

                context.SaveChanges();
            }
            #endregion
        }
    }
}
