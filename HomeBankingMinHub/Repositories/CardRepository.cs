using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) {}
        //public IEnumerable<Card> GetAllCards()
        //{
        //    return FindAll().ToList();
        //}
        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
        public Card FindById(long id)
        {
            return FindByCondition(card => card.Id == id).FirstOrDefault();
        }
        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return FindByCondition(card => card.ClientId == clientId).ToList();
        }

    }
}
