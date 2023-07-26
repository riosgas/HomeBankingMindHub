using HomeBankingMindHub.Models;
using System;
using System.Linq;

namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
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

            if (!context.Accounts.Any())
            {
                var clientVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (clientVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = clientVictor.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 100 },
                        new Account {ClientId = clientVictor.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 },
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
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 100 },
                        new Account {ClientId = clientJuan.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 },
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
