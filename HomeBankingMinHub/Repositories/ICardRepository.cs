using HomeBankingMindHub.Models;
using System.Collections.Generic;

namespace HomeBankingMindHub.Repositories
{
    public interface ICardRepository
    {
        //IEnumerable<Account> GetAllCards(long id);
        void Save(Card card);
        Card FindById(long id);
        IEnumerable<Card> GetCardsByClient(long clientId);
    }
}
