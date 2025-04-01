using Microsoft.AspNetCore.Mvc;
using Trello.DTOs;
using Trello.Service.IService;

namespace Trello.Controller
{
    [Route("api/boards")]
    [ApiController]
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
