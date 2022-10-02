using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Commands;
using BlogApp.DataAccess;
using BlogApp.Domain;
using BlogApp.Implementations.Exceptions;
using BlogApp.Implementations.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Command
{
    public class EfCreateUserFollowerCommand : EfBase, ICreateUserFollowerCommand
    {
        private UserFollowerValidator _validator;
        public EfCreateUserFollowerCommand(BlogAppDbContext context, UserFollowerValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 49;

        public string Name => "EfCreateUserFollowerCommand";

        public string Description =>  "";

        public bool AdminOnly => false;

        public void Execute(UserFollowerDto request)
        {
            var result = this._validator.Validate(request);
            if (result.IsValid)
            {
                var userFollower = new UserFollower
                {
                    UserId = request.UserId,
                    FollowerId = request.FollowerId
                };

                this.context.UsersFollowers.Add(userFollower);
                this.context.SaveChanges();
            }else
            {
                throw new UnproccessableEntityException(result.Errors);
            }
        }
    }
}
