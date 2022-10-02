using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Commands;
using BlogApp.DataAccess;
using BlogApp.Implementations.Exceptions;
using BlogApp.Implementations.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Command
{
    public class EfUpdateUseCaseCommand : EfBase, IUpdateUseCaseCommand
    {
        private UpdateUseCaseValidator _validator;
        public EfUpdateUseCaseCommand(BlogAppDbContext context, UpdateUseCaseValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 44;

        public string Name => "EfUpdateUseCaseCommand";

        public string Description => "";

        public bool AdminOnly => true;

        public void Execute(UseCaseDto request)
        {
            var result = this._validator.Validate(request);
            if (!result.IsValid)
            {
                throw new UnproccessableEntityException(result.Errors);
            }

            var usecase = this.context.UseCases.Find(request.Id);

            usecase.Name = request.Name;
            this.context.SaveChanges();

        }
    }
}
