using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Controllers
{
    [Route("api")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private ICardRepository _cardRepository;
        private IClientRepository _clientRepository;
        public CardsController(IClientRepository clientRepository, ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
        }
        public class CardData
        {
            public string Type { get; set; }
            public string Color { get; set; }
            public CardData(string type, string color)
            {
                Type = type;
                Color = color;
            }
        }

        [HttpPost("clients/current/cards")]
        public IActionResult CreateCardToCurrent([FromBody] CardData card)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client currentClient = _clientRepository.FindByEmail(email);
                if (currentClient == null)
                {
                    return Forbid();
                }

                CardType cardType;
                if (!Enum.TryParse(card.Type, out cardType))
                {
                    return StatusCode(403, $"El tipo de tarjeta {card.Type} no es válido");
                }
                CardColor cardColor;
                if (!Enum.TryParse(card.Color, out cardColor))
                {
                    return StatusCode(403, $"El color de tarjeta {card.Color} no es válido");
                }

                int numberOfCards = currentClient.Cards.Where(c => c.Type == card.Type).Count();
                if (numberOfCards >= 3)
                {
                    return StatusCode(403, $"El cliente tiene 3 tarjetas de tipo {card.Type}, no es posible crear otra");
                }

                static string RandomNumber(int digits)
                {
                    Random random = new Random();
                    if (digits <= 0)
                    {
                        throw new ArgumentException("Debe ser mayor que cero.", nameof(digits));
                    }
                    int min = (int)Math.Pow(10, digits - 1);
                    int max = (int)Math.Pow(10, digits);

                    int randomNumber = random.Next(min, max);
                    return randomNumber.ToString();
                }

                ///pending check existing card number
                Card newCard = new Card
                {
                    ClientId = currentClient.Id,
                    CardHolder = $"{currentClient.FirstName} { currentClient.LastName}",
                    Type = cardType.ToString(),
                    Color = cardColor.ToString(),
                    Number = $"{RandomNumber(4)}-{RandomNumber(4)}-{RandomNumber(4)}-{RandomNumber(4)}",
                    Cvv = int.Parse(RandomNumber(3)),
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(4),
                };
                _cardRepository.Save(newCard);

                return Created("Tarjeta creada exitosamente", newCard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("clients/current/cards")]
        public IActionResult GetCurrentCards()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client currentClient = _clientRepository.FindByEmail(email);
                if (currentClient == null)
                {
                    return Forbid();
                }

                var userCards = currentClient.Cards.Select(c => new CardDTO
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

                return Ok(userCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
