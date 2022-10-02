using BlogApp.API.Core;
using BlogApp.API.Core.Dto;
using BlogApp.API.Extensions;
using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Commands;
using BlogApp.Application.UseCases.Queries;
using BlogApp.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private UseCaseHandler _handler;
        private List<string> _allowedExtensions = new List<string>() { ".jpg", ".png", ".jpeg" };
        public UsersController(UseCaseHandler useCaseHandler)
        {
            this._handler = useCaseHandler;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult Get([FromServices] IGetUsersQuery getUsers)
        {
            return this.HandleUseCase(() =>
            {
                return Ok(this._handler.HandleQuery(getUsers));
            });
        }
        [HttpGet("short/{id}")]
        public IActionResult Get(int id, [FromServices] IFindUserShortQuery query) 
        {
            return this.HandleUseCase(() =>
            {
                return Ok(this._handler.HandleQuery(query, id));
            });
        }
        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IFindUserQuery query)
        {
            return this.HandleUseCase(() =>
            {
                return Ok(this._handler.HandleQuery(query, id));
            });
        }

        // POST api/<UsersController>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] RegisteredUserDto dto, [FromServices] ICreateUserCommand createUser)
        {
            return this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(createUser, dto);
                return StatusCode(201);
            });
        }
        [HttpPost("image")]
        public IActionResult Post([FromForm] UsersImageDto dto, [FromServices] IWebHostEnvironment env)
        {
            return this.HandleUseCase(() =>
            {
                 if(dto.Image != null)
                {
                    var extension = Path.GetExtension(dto.Image.FileName);
                    if (!this._allowedExtensions.Contains(extension))
                    {
                        throw new InvalidOperationException("File extension is unacceptable.");
                    }
                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine("wwwroot", "images", fileName);
                    using(var image = Image.Load(dto.Image.OpenReadStream()))
                    {
                        int[] size = this.GetNewSize(image);
                        image.Mutate(h => h.Resize(size[0], size[1]));
                        image.Save(filePath);
                    }
                    return Ok(new { image = fileName });
                }else
                {
                    return UnprocessableEntity();
                }
            });
        }

        private int[] GetNewSize(Image image, int maxWidth = 250, int maxHeight = 250)
        {
            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                double widthRatio = (double)image.Width / maxWidth;
                double heightRatio = (double)image.Height / maxHeight;
                double ratio = Math.Max(widthRatio, heightRatio);
                int newWidth = (int)(image.Width / ratio);
                int newHeight = (int)(image.Height / ratio);
                return new int[] { newWidth, newHeight };
            }else
            {
                return new int[] { image.Width, image.Height};
            }
        }

        [HttpPatch]
        //[FromForm] -> kada se bude ustanovilo slanje slike
        // UpdateUserWithImageDto
        public IActionResult Patch([FromBody] UpdateUserDto dto, [FromServices] IUpdateUserCommand command)
        {
            return this.HandleUseCase(() =>
            {
                if (dto.ProfilePicture != null)
                {
                    var extension = Path.GetExtension(dto.ProfilePicture);
                    var filePath = Path.Combine("wwwroot", "images", dto.ProfilePicture);
                    if (!this._allowedExtensions.Contains(extension))
                    {
                        throw new InvalidOperationException("File extension is unacceptable.");
                    }
                    if (!System.IO.File.Exists(filePath))
                    {
                        throw new InvalidOperationException("File You wish to upload doesn't exist.");
                    }
                    if (!dto.PreviousProfilePicture.Contains("defaultUserImage"))
                    {
                        filePath = Path.Combine("wwwroot", "images", dto.PreviousProfilePicture);
                        System.IO.File.Delete(filePath);
                    }
                }

                this._handler.HandleCommand(command, dto);
                return NoContent();
            });
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUserCommand deleteUser )
        {
            return this.HandleUseCase(() =>
            {
                this._handler.HandleCommand(deleteUser, id);
                return NoContent();
            });
        }

        [HttpDelete("image/{src}")]
        public IActionResult Delete( string src)
        {
            return this.HandleUseCase(() =>
            {
                var filePath = Path.Combine("wwwroot", "images", src);
                System.IO.File.Delete(filePath);
                return NoContent();
            });
        }
    }

}
