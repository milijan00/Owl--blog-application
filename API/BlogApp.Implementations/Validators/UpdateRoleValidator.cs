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
    public class UpdateRoleValidator : AbstractValidator<RoleWithUsecasesDto>
    {
        public UpdateRoleValidator(BlogAppDbContext context)
        {
            RuleFor(x => x.Name)
                .Must(name => !context.Roles.Any(r => r.Name == name)).WithMessage("There is already a role with given name.");

            RuleFor(x => x.Usecases)
                .Cascade(CascadeMode.Stop)
                .Must(usecases => usecases.Distinct().Count() == usecases.Count()).WithMessage("There are some duplicates in the array.")
                .Must(usecases => usecases.Where(u => context.UseCases.Any(U => u.UsecaseId == U.Id)).Count() == usecases.Count()).WithMessage("Some usecases don't exist.");
        }
    }
}
