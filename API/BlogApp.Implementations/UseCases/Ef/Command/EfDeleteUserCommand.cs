using BlogApp.Application.UseCases.Commands;
using BlogApp.DataAccess;
using BlogApp.DataAccess.Extensions;
using BlogApp.Domain;
using BlogApp.Implementations.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Command
{
    public class EfDeleteUserCommand : EfBase, IDeleteUserCommand
    {
        public EfDeleteUserCommand(BlogAppDbContext context) : base(context)
        {
        }

        public string Name => "EfDeleteUser";

        public string Description => "Delete a specific user via entity framework.";

        public int Id => 17;

        public bool AdminOnly => false;

        public void Execute(int request)
        {
            var user = this.context.Users.Include(user => user.CreatedPosts).Include(u => u.CreatedComments).FirstOrDefault(x => x.Id == request);
            if(user == null)
            {
                throw new EntityNotFoundException(nameof(User), request);
            }
            var likesOnComments = this.context.LikedComments.Where(x => x.UserId == user.Id).ToList();
            this.context.LikedComments.RemoveRange(likesOnComments);

            var likesOnPosts = this.context.LikedPosts.Where(x => x.UserId == user.Id).ToList();
            this.context.LikedPosts.RemoveRange(likesOnPosts);

            var followers = this.context.UsersFollowers.Where(x => x.FollowerId == user.Id || x.UserId == user.Id).ToList();
            this.context.UsersFollowers.RemoveRange(followers);

            this.context.DeactivateRange(user.CreatedComments);
            this.context.DeactivateRange(user.CreatedPosts);
            this.context.Deactivate<User>(request);
            this.context.SaveChanges();
        }
    }
}
