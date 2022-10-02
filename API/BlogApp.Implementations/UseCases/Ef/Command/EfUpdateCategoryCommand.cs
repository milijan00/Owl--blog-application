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
    public class EfUpdateCategoryCommand : EfBase,  IUpdateCategoryCommand
    {
        private CategoryValidator _validator;
        public EfUpdateCategoryCommand(BlogAppDbContext context, CategoryValidator validator)
            : base(context)
        {
            _validator = validator;
        }

        public string Name => "EfUpdateCategoryCommand";

        public string Description => "Update a category via entity framework";

        public int Id =>18;

        public bool AdminOnly => true;

        public void Execute(CategoryDto request)
        {
            var result = this._validator.Validate(request);
            if (!result.IsValid)
            {
                throw new UnproccessableEntityException(result.Errors);
            }
            var category = this.context.Categories.Find(request.Id.Value);

            if(category == null)
            {
                throw new EntityNotFoundException(nameof(Category), request.Id.Value);
            }
            category.Name = request.Name;
            this.context.SaveChanges();
        }
    }
}
