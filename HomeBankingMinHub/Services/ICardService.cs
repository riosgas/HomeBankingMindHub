using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBankingMindHub.Services
{
    public interface ICardService
    {
        public CardDTO CreateCardForCurrentClient(string cardType, string cardColor);
        public List<CardDTO> GetCurrentClientCards();
    }
}
