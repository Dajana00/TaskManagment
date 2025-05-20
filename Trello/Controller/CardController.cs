using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.DTOs;
using Trello.ExeptionHandlingResultFilter;
using Trello.Service.IService;

namespace Trello.Controller
{
    [ResultFilter]
    [Route("api/card")]
    [ApiController]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ICardSprintService _cardSprintService; 

        public CardController(ICardService cardService, ICardSprintService cardSprintService)
        {
            _cardService = cardService;
            _cardSprintService = cardSprintService; 
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCardDto cardDto)
        {
            var response = await _cardService.CreateAsync(cardDto);
            return Ok(response);

        }
        [HttpPut("move")]
        public async Task<IActionResult> MoveCard([FromBody] MoveCardDto dto)
        {
            var response = await _cardService.UpdateCardStatus(dto);
            return Ok(response);
        }

        [HttpGet("getByBoardId/{boardId}")]
        public async Task<IActionResult> GetByBoardId([FromRoute] int boardId)
        {
            var response = await _cardService.GetByBoardId(boardId);
            return Ok(response.Value);
        }
        [HttpGet("getByUserStoryId/{userStoryId}")]
        public async Task<IActionResult> GetByUserStoryId([FromRoute]int userStoryId)
        {
            var response = await _cardService.GetByUserStoryId(userStoryId);
            return Ok(response.Value);
        }
        [HttpPut("addToActiveSprint/{id}")]
        public async Task<IActionResult> AddToActiveSprint([FromRoute] int id)
        {
            var response = await _cardSprintService.AddCardToActiveSprint(id);
            return Ok(response);
        }

        [HttpDelete("delete/{cardId}")]
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            var result = await _cardService.Delete(cardId);
            return NoContent();
        }

        [HttpPut("update/{cardId}")]
        public async Task<IActionResult> UpdateCard(int cardId, [FromBody] UpdateCardDto dto)
        {
            var result = await _cardService.Update(cardId, dto);
            return Ok(result.Value);
        }

    }


}
