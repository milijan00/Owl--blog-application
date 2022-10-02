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
    public class UseCaseController : ControllerBase
    {
        private UseCaseHandler _handler;
        public UseCaseController(UseCaseHandler handler) => this._handler = handler;

        [HttpGet]
        public IActionResult Get([FromServices] IGetUseCasesQuery query) 
        {
            return this.HandleUseCase(() =>
            {
                return Ok(this._handler.HandleQuery(query));
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IFindUseCaseQuery query)
        {
            return this.HandleUseCase(() =>
           {
               return Ok(this._handler.HandleQuery(query, id));
           });
        }
        [HttpPost]
        public IActionResult Post([FromBody] UseCaseDto dto, [FromServices] Application.UseCases.Commands.ICreateUseCasesCommand command)  
        {
           return   this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(command, dto);
                return StatusCode(201);
            });
        }

        [HttpPut]
        public IActionResult Put([FromBody] UseCaseDto dto, [FromServices] IUpdateUseCaseCommand command) 
        {
            return this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(command, dto);
                return NoContent();
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUseCaseCommand command)
        {
            return this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(command, id);
                return NoContent();
            });
        }
    }
}
