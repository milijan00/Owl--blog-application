using BlogApp.Application.Dto;
using BlogApp.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.Validators
{
    public class PostValidator : AbstractValidator<PostDto>
    {
        private BlogAppDbContext _context;
        public PostValidator()
        {
            this._context = new BlogAppDbContext();
            RuleFor(x => x.Title).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Title must not be null or empty")
                .MinimumLength(2).WithMessage("Minimum number of characters is 2.")
                .MaximumLength(100).WithMessage("Maximum number of characters is 100.");
                //.Matches(@"^[A-z0-9\s]{2,}$");
            RuleFor(x => x.Content).Cascade(CascadeMode.Stop)
                .MinimumLength(2).WithMessage("Minimum number of characters is 2.");
            RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("UserId must not be null.")
                //.Must(id => id > 0).WithMessage("User id less than or equal to 0.")
                .Must(id => this._context.Users.Any(u => u.Id == id && u.IsActive)).WithMessage("Given user doesn't exist.");

            RuleFor(x => x.CategoryId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("CategoryId must not be null.")
                //.Must(id => id > 0).WithMessage("Category id must not be less than or equal to 0.")
                .Must(id => this._context.Categories.Any(c => c.Id == id && c.IsActive)).WithMessage("Given category doesn't exist.");

        }
    }
}
