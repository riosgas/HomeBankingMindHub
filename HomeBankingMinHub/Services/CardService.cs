using HomeBankingMindHub.Enums;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using System.Threading.Tasks;
using System;
using System.Linq;
using HomeBankingMindHub.dtos;
using System.Collections.Generic;
using HomeBankingMindHub.Helpers;

namespace HomeBankingMindHub.Services
{
    public class CardService : ICardService
    {
        private ICardRepository _cardRepository;
        private IClientRepository _clientRepository;
        private readonly IAuthService _authService;
        public CardService(ICardRepository cardRepository, IClientRepository clientRepository, IAuthService authService)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
            _authService = authService;
        }

        public CardDTO CreateCardForCurrentClient(string cardType, string cardColor)
        {
            string UserAuthenticatedEmail = _authService.UserAuthenticated();
            if (UserAuthenticatedEmail == null) return null;

            Client currentClient = _clientRepository.FindByEmail(UserAuthenticatedEmail);
            if (currentClient == null) throw new Exception("Cliente no encontrado");

            CardType parsedCardType;
            if (!Enum.TryParse(cardType, out parsedCardType))
                throw new Exception($"El tipo de tarjeta {cardType} no es válido");

            CardColor parsedCardColor;
            if (!Enum.TryParse(cardColor, out parsedCardColor))
                throw new Exception($"El color de tarjeta {cardColor} no es válido");

            int numberOfCards = currentClient.Cards.Count(c => c.Type == parsedCardType.ToString());
            if (numberOfCards >= 3)
                throw new Exception($"El cliente tiene 3 tarjetas de tipo {parsedCardType}, no es posible crear otra");

            (string number, int cvv) cardData = CardDataGeneratorHelper.Generate();
            ///pending check existing card numbers
            Card newCard = new Card
            {
                ClientId = currentClient.Id,
                CardHolder = $"{currentClient.FirstName} {currentClient.LastName}",
                Type = cardType.ToString(),
                Color = cardColor.ToString(),
                Number = cardData.number,
                Cvv = cardData.cvv,
                FromDate = DateTime.Now,
                ThruDate = DateTime.Now.AddYears(4),
            };
            _cardRepository.Save(newCard);

            newCard = _cardRepository.FindByNumber(newCard.Number);
            CardDTO newCardDTO = new CardDTO
            {
                CardHolder = newCard.CardHolder,
                Type = newCard.Type,
                Color = newCard.Color,
                Number = newCard.Number,
                Cvv = newCard.Cvv,
                FromDate = newCard.FromDate,
                ThruDate = newCard.ThruDate,
            };
            return newCardDTO;
        }
        public List<CardDTO> GetCurrentClientCards()
        {
            string UserAuthenticatedEmail = _authService.UserAuthenticated();
            if (UserAuthenticatedEmail == null) return null;

            Client currentClient = _clientRepository.FindByEmail(UserAuthenticatedEmail);
            if (currentClient == null) throw new Exception("Cliente no encontrado");

            List<CardDTO> userCards = currentClient.Cards.Select(c => new CardDTO
            {
                Id = c.Id,
                CardHolder = c.CardHolder,
                Type = c.Type,
                Color = c.Color,
                Number = c.Number,
                Cvv = c.Cvv,
                FromDate = c.FromDate,
                ThruDate = c.ThruDate,
            }).ToList();

            return userCards;
        }
    }
}
