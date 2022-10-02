using BlogApp.Application.Dto;
using BlogApp.Application.Dto.searches;
using BlogApp.Application.UseCases.Queries;
using BlogApp.DataAccess;
using BlogApp.DataAccess.Exceptions;
using BlogApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Implementations.UseCases.Ef.Query
{
    public class EfFindPostQuery : EfBase, IFindPostQuery
    {
        public EfFindPostQuery(BlogAppDbContext context) : base(context)
        {
        }

        public int Id => 29;

        public string Name => "EfFindPost";

        public string Description => "Find a specific post via entity framework.";

        public bool AdminOnly => false;

        public SearchResultPostDto Execute(int id)
        {
            var post = this.context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.UsersWhoLiked)
                .FirstOrDefault(p => p.Id == id && p.IsActive);
            if(post == null)
            {
                throw new EntityNotFoundException();
            }
            var createdAt = $"{post.CreatedAt.Date.Day}/{post.CreatedAt.Date.Month}/{post.CreatedAt.Date.Year}";
            return new SearchResultPostDto
            {
                Id = post.Id,
                Content = post.Content,
                CategoryId = post.CategoryId,
                Category = post.Category.Name,
                CreatedAt = createdAt,
                Title = post.Title,
                UserId = post.UserId,
                Likes = post.UsersWhoLiked.Select(x => new
                {
                    PostId = x.PostId,
                    UserId = x.UserId
                }).ToList(),
                ProfilePicture = post.Author.ProfilePicture,
               Firstname = post.Author.Firstname  ,
               Lastname = post.Author.Lastname,
               Images = post.PostsImages.Select(x => new PostImageDto
               {
                    Id = x.Id,
                    PostId = x.PostId,
                    Source = x.Source
               }).ToList()
            };

        }
    }
}
