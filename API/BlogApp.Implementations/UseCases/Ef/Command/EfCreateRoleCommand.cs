using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Commands;
using BlogApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.DataAccess;
using BlogApp.Implementations.Validators;
using BlogApp.Implementations.Exceptions;

namespace BlogApp.Implementations.UseCases.Ef.Command
{
    public class EfCreateRoleCommand : EfBase,  ICreateRoleCommand
    {
        private RoleValidator _validator;
        public EfCreateRoleCommand(BlogAppDbContext context, RoleValidator validator) : base(context)
        {
            _validator = validator;
        }
        public string Name => "EfCreateRole";

        public string Description => "Create a new Role";

        public int Id => 7;

        public bool AdminOnly => true;

        public void Execute(RoleDto request)
        {
            var result = this._validator.Validate(request);
            if (result.IsValid)
            {
                var role = new Role()
                {
                    Name = request.Name
                };
                this.context.Roles.Add(role);
                this.context.SaveChanges();
            }
            else
            {
                //foreach(var error in result.Errors)
                //{
                //    Console.WriteLine($"{error.PropertyName} : {error.ErrorMessage}");
                //}
                throw new UnproccessableEntityException(result.Errors);
            }
        }
    }
}
