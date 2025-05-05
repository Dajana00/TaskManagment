using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.Filters;
using Trello.Service.IService;

namespace Trello.Controller
{
    [ResultFilter]
    [Route("api/boards")]
    [ApiController]
    [Authorize]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

       
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _boardService.GetById(id);
            return Ok(response.Value);

        }
    }
}
