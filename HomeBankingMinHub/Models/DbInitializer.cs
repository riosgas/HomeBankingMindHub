using HomeBankingMinHub.Models;
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

                //guardamos
                context.SaveChanges();
            }

        }
    }
}