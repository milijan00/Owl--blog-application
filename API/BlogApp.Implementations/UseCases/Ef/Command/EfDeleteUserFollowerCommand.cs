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
    public class EfDeleteUserFollowerCommand : EfBase, IDeleteUserFollowerCommand
    {
        private UserFollowerValidator _validator;
        public EfDeleteUserFollowerCommand(BlogAppDbContext context, UserFollowerValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 50;

        public string Name => "EfDeleteUserFollowerCommand";

        public string Description => "";

        public bool AdminOnly => false;

        public void Execute(UserFollowerDto request)
        {
            var result = this._validator.Validate(request);
            if (!result.IsValid)
            {
                throw new UnproccessableEntityException(result.Errors);
            }

           var userFollower = this.context.UsersFollowers.FirstOrDefault(uf => uf.UserId == request.UserId && uf.FollowerId == request.FollowerId);
            if(userFollower == null)
            {
                throw new EntityNotFoundException(nameof(UserFollower), request);
            }

            this.context.UsersFollowers.Remove(userFollower);
            this.context.SaveChanges();

        }
    }
}
