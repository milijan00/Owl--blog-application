using BlogApp.API.Extensions;
using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Commands;
using BlogApp.Application.UseCases.Queries;
using BlogApp.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowersController : ControllerBase
    {
        private UseCaseHandler _handler;
        public FollowersController(UseCaseHandler handler)
        {
            _handler = handler;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IGetFollowersFollowingQuery query)
        {
            return this.HandleUseCase(() =>
            {
                return Ok(this._handler.HandleQuery(query, id));
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserFollowerDto dto, [FromServices] ICreateUserFollowerCommand command)
        {
            return this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(command, dto);
                return StatusCode(201);
            });
        }

        [HttpDelete("user/{userId}/follower/{followerId}")]
        public IActionResult Delete(int userId, int followerId, [FromServices] IDeleteUserFollowerCommand command) 
        {
            return this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(command, new UserFollowerDto
                {
                    UserId = userId,
                    FollowerId = followerId
                });
                return NoContent();
            });
        }
    }
}
