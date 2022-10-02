using BlogApp.DataAccess;
using BlogApp.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitialDataController : ControllerBase
    {
        private BlogAppDbContext _context;
        public InitialDataController(BlogAppDbContext context) => this._context = context;
        [HttpPost]
        public IActionResult Post()
        {
            if (this._context.Roles.Any())
            {
                return Conflict();
            }

            var roles = new List<Role>
            {
                new Role() {Name="Administrator"},
                new Role(){Name = "User"}
            };
            var categories = new List<Category>
            {
                new Category() {Name="Sports"},
                new Category() {Name="Science"},
                new Category() {Name="Phylosophy"},
                new Category() {Name="Psychology"}
            };

            var users = new List<User>
            {
                new User()
                {
                    Lastname = "Milijanovic",
                    Firstname = "Aleksandar",
                    Email = "aleksandar.milijanovic.91.19@ict.edu.rs",
                    Username = "milijan",
                    Password = BCrypt.Net.BCrypt.HashPassword("Milijan00"),
                    Role = roles.First(),
                    ProfilePicture ="defaultUserImage.jpg"
                }
            };
            this._context.Roles.AddRange(roles);
            this._context.Categories.AddRange(categories);
            this._context.Users.AddRange(users);
            this._context.SaveChanges();
            return StatusCode(201);
        }
    }
}
