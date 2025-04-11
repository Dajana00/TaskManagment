﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.DTOs;
using Trello.Service.IService;

namespace Trello.Controller
{
    [Route("api/card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CardDto cardDto)
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

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _cardService.GetAll();
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
            var response = await _cardService.AddToActiveSprint(id);
            return Ok(response);
        }
    }


}
