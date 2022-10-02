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
    public class EfGetFollowersFollowingQuery : EfBase, IGetFollowersFollowingQuery
    {
        public EfGetFollowersFollowingQuery(BlogAppDbContext context) : base(context)
        {
        }

        public int Id => 48;

        public string Name => "EfGetFollowersFollowingQuery";

        public string Description => "";

        public bool AdminOnly => false;

        public FollowersFollowingDto Execute(int request)
        {
            var user = this.context.Users.Find(request);
            if(user == null)
            {
                throw new EntityNotFoundException(nameof(User), request);
            }
            return new FollowersFollowingDto
            {
                Followers = this.context.UsersFollowers.Where(uf => uf.UserId == request).Select(x => new FollowerDto
                {
                   Id = x.Follower.Id,
                   Firstname = x.Follower.Firstname,
                   Lastname = x.Follower.Lastname,
                   ProfilePicture = x.Follower.ProfilePicture
                }).ToList(),
                Following = this.context.UsersFollowers.Where(uf => uf.FollowerId == request).Select(x => new FollowerDto
                {
                    Id = x.User.Id,
                    Firstname = x.User.Firstname,
                    Lastname = x.User.Lastname,
                    ProfilePicture = x.User.ProfilePicture
                }).ToList()
            };
        }
    }
}
