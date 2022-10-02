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
    public class CreateUseCaseValidator : AbstractValidator<UseCaseDto>
    {
        public CreateUseCaseValidator(BlogAppDbContext context)
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Usecase name must not be null or empty.")
                .Matches("^[A-Za-z]{5,60}$").WithMessage("Only letters are acceptable. Names length needs to be between 5 and 60 characters.")
                .Must(usecaseName =>  !context.UseCases.Any(x => x.Name == usecaseName)).WithMessage("There is already an usecase with given name.");
        }
    }
}
