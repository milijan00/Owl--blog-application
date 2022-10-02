using BlogApp.API.Extensions;
using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Commands;
using BlogApp.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleUseCasesController : ControllerBase
    {
        private UseCaseHandler _handler;

        public RoleUseCasesController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpPatch] 
        [AllowAnonymous]
       public IActionResult Patch([FromBody] UpdateUseCasesDto dto, [FromServices] IUpdateUseCasesCommand command)
        {
            return this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(command, dto);
                return NoContent();
            });
        }
    }
}
