using BlogApp.DataAccess;
using BlogApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BlogApp.API.Core
{
    public class JwtManager
    {
        private readonly BlogAppDbContext _context;
        private readonly JwtSettings _settings;

        public JwtManager(BlogAppDbContext context, JwtSettings settings)
        {
            _settings = settings;
            _context = context;
        }

        public List<string> MakeTokens(int id)
        {
            var user = _context.Users.Where(u => u.IsActive).Include(x => x.Role).ThenInclude(r => r.UseCases).FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return new List<string>() { this.GenerateAccessToken(user), this.GenerateRefreshToken(user) };
        }
        public List<string> MakeTokens(string email, string password)
        {
            var user = _context.Users.Where(u => u.IsActive).Include(x => x.Role).ThenInclude(r => r.UseCases).FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var valid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if(!valid)
            {
                throw new UnauthorizedAccessException();
            }

            return new List<string>() { this.GenerateAccessToken(user), this.GenerateRefreshToken(user) };
        }
        private string GenerateAccessToken(User user)
        {
            var actor = new JwtUser
            {
                Id = user.Id,
                AllowedUseCasesIds = user.Role.UseCases.Select(x => x.UseCaseId).ToList(),
                Identity = user.Role.Name,
                Email = user.Email
            };

            var claims = new List<Claim> // Jti : "",
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _settings.Issuer),
                new Claim("UserId", actor.Id.ToString(), ClaimValueTypes.String, _settings.Issuer),
                new Claim("UseCases", JsonConvert.SerializeObject(actor.AllowedUseCasesIds)),
                new Claim("Role", user.Role.Name),
                new Claim("Email", user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.AccessSecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: "Any",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_settings.Minutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(User user)
        {
            var actor = new JwtUser
            {
                Id = user.Id,
                Identity = user.Role.Name
            };

            var claims = new List<Claim> // Jti : "",
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _settings.Issuer),
                new Claim("UserId", actor.Id.ToString(), ClaimValueTypes.String, _settings.Issuer)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.RefreshSecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: "Any",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_settings.RefreshTokenMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
