using BlogApp.Application.Dto;
using BlogApp.Application.Dto.searches;
using BlogApp.Application.UseCases;
using BlogApp.Application.UseCases.Commands;
using BlogApp.Application.UseCases.Queries;
using BlogApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Query
{
    public class EfGetPostsQuery : EfBase, IGetPostsQuery
    {
        public EfGetPostsQuery(BlogAppDbContext context) : base(context)
        {
        }

        public string Name => "EfGetPosts";

        public string Description => "Get all posts with optional search via entity framework.";

        public int Id => 38;

        public bool AdminOnly => false;

        public SearchResult<SearchResultPostDto> Execute(SearchPostDto request)
        {
            var query = this.context.Posts.Where(p => p.IsActive).Include(p => p.Author).Include(p => p.UsersWhoLiked).Include(p => p.Category).Include(p => p.Comments).AsQueryable();

            var response = new SearchResult<SearchResultPostDto>();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(p => p.Title.Contains(request.Keyword) || p.Author.Firstname.Contains(request.Keyword) || p.Author.Lastname.Contains(request.Keyword) || p.Author.Username.Contains(request.Keyword));
            }
            if (request.Categories.Count() > 0)
            {
                query = query.Where(p => request.Categories.Contains(p.CategoryId));
            }

            if(request.Page == null || request.Page.Value < 1)
            {
                request.Page = 1;
            }

            if(request.PerPage == null || request.PerPage.Value < 1)
            {
                request.Page = 20;
            }

            var postsToSkip = (request.Page.Value - 1) * request.PerPage.Value;
            
            response.TotalCount = query.Count();
            response.TotalPages = (int)Math.Ceiling((decimal)query.Count() / request.PerPage.Value);
            response.Data =  query.Skip(postsToSkip).Take(request.PerPage.Value).OrderByDescending(p => p.CreatedAt).Select(p => new SearchResultPostDto
            {
                Id = p.Id,
                Content = p.Content,
                CategoryId = p.CategoryId,
                Category = p.Category.Name,
                Title = p.Title,
                UserId = p.UserId,
                CreatedAt = $"{p.CreatedAt.Date.Day}/{p.CreatedAt.Date.Month}/{p.CreatedAt.Date.Year}",
                ProfilePicture = p.Author.ProfilePicture,
               Firstname = p.Author.Firstname,
               Lastname = p.Author.Lastname,
               Username = p.Author.Username,
               NumberOfLikes = p.UsersWhoLiked.Count,
               Images = p.PostsImages.Where(i => i.IsActive).Select(x => new PostImageDto
               {
                    Id = x.Id,
                    PostId = x.PostId,
                    Source = x.Source
               }).ToList(),
               Comments = p.Comments.Where(c => c.IsActive).Select(x => new CommentDto
               {
                    Id = x.Id,
                    Content = x.Content,
                    ParentId = x.ParentId,
                    UserId = x.UserId,
                    PostId = x.PostId
               }).ToList()
            }) .ToList();
            response.CurrentPage = request.Page.Value;
            response.ItemsPerPage = request.PerPage.Value;

            return response;
        }

         
    }
}
