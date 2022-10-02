using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Queries;
using BlogApp.DataAccess;
using BlogApp.Domain;
using BlogApp.Implementations.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Query
{
    public class EfFindUserShortQuery : EfBase, IFindUserShortQuery
    {
        public EfFindUserShortQuery(BlogAppDbContext context) : base(context)
        {
        }

        public int Id => 47;

        public string Name => "EfFindUserShortQuery";

        public string Description => "This usecase provides only information from 'Users' table.";

        public bool AdminOnly => false;

        public RegisteredUserDto Execute(int request)
        {
            var user = this.context.Users.Find(request);
            if(user == null)
            {
                throw new EntityNotFoundException(nameof(User), request);
            }
            var password = "";
            return new RegisteredUserDto
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username,
                Password =  password,
                ImageSource = user.ProfilePicture
            };
        }
    }
}
