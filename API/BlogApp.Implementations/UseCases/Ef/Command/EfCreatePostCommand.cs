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
    public class EfCreatePostCommand : EfBase, ICreatePostCommand
    {
        private PostValidator _validator;
        public EfCreatePostCommand(BlogAppDbContext context, PostValidator validator) : base(context)
        {
            _validator = validator;
        }


        public string Name => "EfCreatePostCommand";

        public string Description => "Create a  post via entity framework";

        public int Id => 4;

        public bool AdminOnly => false;

        public void Execute(PostDto request)
        {
            this._validator = new PostValidator();
            var result = this._validator.Validate(request);
            if (result.IsValid)
            {
                var post = new Post
                {
                    UserId = request.UserId.Value,
                    CategoryId = request.CategoryId.Value,
                    Title = request.Title,
                    Content = request.Content
                };
                this.context.Posts.Add(post);
                this.context.SaveChanges();
            }
            else
            {
                throw new UnproccessableEntityException(result.Errors);
            }
        }

    }
}
