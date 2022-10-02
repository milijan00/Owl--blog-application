using AutoMapper;
using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Queries;
using BlogApp.DataAccess;
using BlogApp.Domain;
using BlogApp.Implementations.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Query
{
    public class EfFindUserQuery : EfBase, IFindUserQuery
    {
        private IMapper _mapper;
        public EfFindUserQuery(BlogAppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public int Id => 31;

        public string Name => "EfFindUser";

        public string Description => "Find a user via entity framework";

        public bool AdminOnly => false;

        public UserDto Execute(int request)
        {
            var user = this.context.Users.Include(u => u.Followers).ThenInclude(x => x.Follower).Include(u => u.Following).ThenInclude(u => u.User).FirstOrDefault(u => u.Id == request && u.IsActive);
            if(user == null)
            {
                throw new EntityNotFoundException(nameof(User), request);
            }
            var posts = this.context.Posts.Where(p => p.IsActive && p.UserId == user.Id)
                .Include(p => p.Comments)
                .Include(p => p.UsersWhoLiked)
                .OrderByDescending(x => x.CreatedAt)
                .Select(p => new UsersPostDto
                {
                    Id = p.Id,
                    ProfilePicture = user.ProfilePicture,
                    Username = user.Username,
                    Title = p.Title,
                    Content = p.Content,
                    NumberOfComments = p.Comments.Count(),
                    NumberOfLikes = p.UsersWhoLiked.Count(),
                    Category = p.Category.Name,
                    UserId = p.UserId,
                    CreatedAt = $"{p.CreatedAt.Date.Day}/{p.CreatedAt.Date.Month}/{p.CreatedAt.Date.Year}"
                
            }).ToList();
            //return this._mapper.Map<UserDto>(user);
            return new FoundUserDto
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Posts = posts,
                Username = user.Username,
                ProfilePicture = user.ProfilePicture,
                Followers = user.Followers.Select(x => new FollowerDto
                {
                    Id = x.Follower.Id,
                    Firstname = x.Follower.Firstname,
                    Lastname = x.Follower.Lastname,
                    Username = x.Follower.Username,
                    ProfilePicture = x.Follower.ProfilePicture
                }).ToList(),
                Following = user.Following.Select(x => new FollowerDto
                {
                    Id = x.User.Id,
                    Firstname = x.User.Firstname,
                    Lastname = x.User.Lastname,
                    Username = x.User.Username,
                    ProfilePicture = x.User.ProfilePicture
                }).ToList()
            };

        }
    }
}
