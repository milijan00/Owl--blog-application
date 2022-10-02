using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Queries;
using BlogApp.DataAccess;
using BlogApp.Implementations.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Query
{
    public class EfGetPostsLikesQuery : EfBase, IGetPostsLikesQuery
    {
        public EfGetPostsLikesQuery(BlogAppDbContext context) : base(context)
        {
        }

        public int Id => 37;

        public string Name => "EfGetPostsLikes";

        public string Description => "Get posts likes  via entity framework.";

        public bool AdminOnly => false;

        public IEnumerable<RequestPostsLikeDto> Execute(int id)
        {
            var post = this.context.Posts.Include(p => p.UsersWhoLiked).ThenInclude(lp => lp.User).FirstOrDefault(p => p.Id == id);
            if(post == null)
            {
                throw new EntityNotFoundException("Post", id);
            }

            return post.UsersWhoLiked.Select(x => new RequestPostsLikeDto
            {
                PostId = x.PostId,
                UserId = x.UserId 
            }).ToList(); 

        }
    }
}
