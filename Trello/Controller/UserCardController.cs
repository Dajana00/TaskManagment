using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Service.IService;

namespace Trello.Controller
{
    [Route("api/userCard")]
    [ApiController]
    [Authorize]
    public class UserCardController : ControllerBase
    {
        private readonly IUserCardService _userCardService;

        public UserCardController(IUserCardService service)
        {
            _userCardService = service;
        }

        [HttpGet("getMembers/{cardId}")]
        public async Task<IActionResult> GetByProjectId([FromRoute] int cardId)
        {
            var response = await _userCardService.GetCardMembersAsync(cardId);
            return Ok(response.Value);
        }

        [HttpGet("getNonMembers/{cardId}/{projectId}")]
        public async Task<IActionResult> GetNonMembers([FromRoute]int cardId, int projectId)
        {
            var response = await _userCardService.GetNonMembers(cardId,projectId);
            return Ok(response.Value);
        }

        [HttpPost("addOnCard/{cardId}/{userId}")]
        public async Task<IActionResult> AddUserOnCard(int cardId, int userId)
        {
            var response = await _userCardService.AddUserToCardAsync(cardId, userId);
            return Ok(response);
        }


    }
}