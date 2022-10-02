using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.DataAccess;
using BlogApp.Implementations.Exceptions;
using BlogApp.Domain;
using BlogApp.Implementations.Validators;

namespace BlogApp.Implementations.UseCases.Ef.Command
{
    public class EfUpdateRoleCommand : EfBase,  IUpdateRoleCommand
    {
        private UpdateRoleValidator _validator;
        public EfUpdateRoleCommand(BlogAppDbContext context, UpdateRoleValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "EfUpdateRole";

        public string Description => "Update a specific role";

        public int Id =>23;

        public bool AdminOnly => true;

        public void Execute(RoleWithUsecasesDto request)
        {
            var result = this._validator.Validate(request);
            if (!result.IsValid)
            {
                throw new UnproccessableEntityException(result.Errors);
            }

            var role = this.context.Roles.Find(request.Id.Value);
            if(role == null)
            {
                throw new EntityNotFoundException(nameof(Role), request.Id.Value);
            }
            role.Name = request.Name;
            var usecasesToBeDeleted = this.context.RolesUseCases.Where(x => x.RoleId == request.Id).ToList();
            this.context.RolesUseCases.RemoveRange(usecasesToBeDeleted);
            var usecasesToBeInserted = request.Usecases.Select(x => new RoleUseCase
            {
                RoleId = x.RoleId,
                UseCaseId = x.UsecaseId
            }).ToList();
            this.context.RolesUseCases.AddRange(usecasesToBeInserted);
            this.context.SaveChanges();
        }
    }
}
