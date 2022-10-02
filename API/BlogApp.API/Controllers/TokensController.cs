using BlogApp.API.Core;
using BlogApp.API.Extensions;
using BlogApp.Application.Dto;
using BlogApp.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private JwtManager _jwtManager;

        public TokensController(JwtManager jwtManager)
        {
            _jwtManager = jwtManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] TokenRequest tokenRequest)
        {
            return this.HandleUseCase(() =>
            {
                var token = this._jwtManager.MakeTokens(tokenRequest.Email, tokenRequest.Password);
                return Ok(new { access = token[0], refresh = token[1] });
            });
        }
        [HttpPost("refresh")]
        public IActionResult Post([FromBody] RefreshTokenDto refreshToken)
        {
            return this.HandleUseCase(() =>
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(refreshToken.Value);
                var userId = token.Payload["UserId"];
                if(token == null || token.Payload["UserId"] == null)
                {
                    return BadRequest();
                }
                var tokens = this._jwtManager.MakeTokens(Convert.ToInt32(token.Payload["UserId"])); 
                return Ok(new {access = tokens[0], refresh = tokens[1]});
            });
        }
    }
}
