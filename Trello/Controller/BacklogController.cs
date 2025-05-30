﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.ExeptionHandlingResultFilter;
using Trello.Service.IService;

namespace Trello.Controller
{
    [ResultFilter]
    [Route("api/backlog")]
    [ApiController]
    [Authorize]
    public class BacklogController : ControllerBase
    {
        private readonly IBacklogService _backlogService;

        public BacklogController(IBacklogService backlogService, IUserStoryService userStoryService)
        {
            _backlogService = backlogService;
        }


        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _backlogService.GetById(id);
            return Ok(response.Value);

        }
    }
}
