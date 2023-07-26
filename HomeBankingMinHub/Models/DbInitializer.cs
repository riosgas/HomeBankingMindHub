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
                    new Client { FirstName="Victor", LastName="Coronado", Email = "vcoronado@gmail.com", Password="123456"},
                    new Client { FirstName="Juan", LastName="Carlos", Email = "jc@gmail.com", Password="123456"}
                };

                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }
                //Client client1 = new Client { FirstName = "Victor", LastName = "Coronado", Email = "vcoronado@gmail.com", Password = "123456" };
                //context.Clients.Add(client1);
            }
            #endregion

            #region Accounts data
            if (!context.Accounts.Any())
            {
                var clientVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (clientVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = clientVictor.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 100 },
                        new Account {ClientId = clientVictor.Id, CreationDate = DateTime.Now, Number = "VIN002", Balance = 0 },
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                var clientJuan = context.Clients.FirstOrDefault(c => c.Email == "jc@gmail.com");
                if (clientJuan != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = "VIN003", Balance = 100 },
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = "VIN004", Balance = 0 },
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
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
            }
            #endregion

            context.SaveChanges();
        }
    }
}
