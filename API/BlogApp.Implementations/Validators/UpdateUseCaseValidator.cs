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
    public class UpdateUseCaseValidator : AbstractValidator<UseCaseDto>
    {
        public UpdateUseCaseValidator(BlogAppDbContext context)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("UseCase id mustn't be null.")
                .Must(id => context.UseCases.Any(u => u.Id == id)).WithMessage("Given usecase doesn't exist.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("UseCase name must not be null or empty.")
                .Matches("^[A-Za-z]{5,60}$").WithMessage("Only letters are acceptable. Names length needs to be between 5 and 60 characters.")
                .Must(name => !context.UseCases.Any(u => u.Name == name)).WithMessage("There is already an UseCase with given name.");
        }
    }
}
