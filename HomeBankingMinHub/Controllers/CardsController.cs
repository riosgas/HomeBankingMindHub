using HomeBankingMindHub.dtos;
using HomeBankingMindHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HomeBankingMindHub.Controllers
{
    [Route("api")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;
        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("clients/current/cards")]
        public IActionResult CreateCardToCurrent([FromBody] CreateCardRequestDTO cardRequest)
        {
            CardDTO newCardDTO = _cardService.CreateCardForCurrentClient(cardRequest.Type, cardRequest.Color);
            if (newCardDTO == null)
                return Unauthorized();

            return Created("Tarjeta creada", newCardDTO);
        }

        [HttpGet("clients/current/cards")]
        public IActionResult GetCurrentCards()
        {
            List<CardDTO> clientCards = _cardService.GetCurrentClientCards();
            if (clientCards == null)
                return Unauthorized();

            return Ok(clientCards);
        }

    }
}
