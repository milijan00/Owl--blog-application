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
    public class UserFollowerValidator : AbstractValidator<UserFollowerDto>
    {
        public UserFollowerValidator(BlogAppDbContext context)
        {
            RuleFor(x => x.UserId)
                .Must(id => context.Users.Any(u => u.Id == id)).WithMessage("There is no such user.");

            RuleFor(x => x.FollowerId)
                .Must(id => context.Users.Any(u => u.Id == id)).WithMessage("There is no such follower.");
        }   
    }
}
