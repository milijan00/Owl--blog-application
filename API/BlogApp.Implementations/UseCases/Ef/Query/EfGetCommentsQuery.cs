using AutoMapper;
using BlogApp.Application.Dto;
using BlogApp.Application.Dto.searches;
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
    public class EfGetCommentsQuery : EfBase, IGetCommentsQuery
    {
        private IMapper _mapper;
        public EfGetCommentsQuery(BlogAppDbContext context,IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public int Id => 34; 

        public string Name => "EfGetComments";

        public string Description => "Search for comments via entity framework";

        public bool AdminOnly => false;

        public IEnumerable<CommentDto> Execute(SearchCommentDto request)
        {
            var query = this.context.Comments
                .Where(c => c.IsActive && c.ParentId == null)
                .Include(c => c.ChildComments)
                .ThenInclude(c => c.UsersWhoLiked)
                .Include(c => c.ChildComments)
                .ThenInclude(child => child.CommentedBy)
                .Include(c => c.CommentedBy)
                .Include(c => c.UsersWhoLiked)
                .Where(c => c.IsActive)
                .AsQueryable();
            if (request.UserId.HasValue)
            {
                query = query.Where(c => c.UserId == request.UserId.Value);
            }
            if (request.PostId.HasValue)
            {
                query = query.Where(c => c.PostId == request.PostId.Value);
            }
            var comments = query.ToList();
            //return this._mapper.Map<List<CommentDto>>(comments);
            return comments.Select(c => new CommentDto
            {
                Content = c.Content,
                Username = c.CommentedBy.Username,
                UserId = c.UserId,
                ChildComments = c.ChildComments.Where(child => child.IsActive).Select(x => new CommentDto
                {
                    Content = x.Content,
                    Username = x.CommentedBy.Username,
                    UserId = x.UserId,
                    Id = x.Id,
                    ParentId = x.ParentId,
                    ProfilePicture = x.CommentedBy.ProfilePicture,
                    Likes = x.UsersWhoLiked.Select(y => new
                    {
                        UserId = y.UserId,
                        CommentId = y.CommentId
                    }).ToList()
                }).ToList(),
                Id = c.Id,
                ParentId = c.ParentId,
                ProfilePicture = c.CommentedBy.ProfilePicture,
                Likes = c.UsersWhoLiked.Select(x => new
                {
                    UserId = x.UserId,
                    CommentId = x.CommentId
                }).ToList()
            }).ToList();
        }
    }
}
