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
    public class EfCreateUseCaseCommand : EfBase, ICreateUseCasesCommand
    {
        private CreateUseCaseValidator _validator;
        public EfCreateUseCaseCommand(BlogAppDbContext context, CreateUseCaseValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 41;

        public string Name => "EfCreateUseCasesCommand";

        public string Description => "";

        public bool AdminOnly => true;

        //public void Execute(CreateUseCasesDto request)
        //{
        //    var result = this._validator.Validate(request);
        //    if (result.IsValid)
        //    {
        //        this.context.UseCases.AddRange(request.UseCases.Select(u => new UseCase
        //        {
        //           Name = u 
        //        }).ToList());
        //        this.context.SaveChanges();
        //    }else
        //    {
        //        throw new UnproccessableEntityException(result.Errors);
        //    }
        //}

        public void Execute(UseCaseDto request)
        {
            var result = this._validator.Validate(request);
            if (result.IsValid)
            {
                var usecase = new UseCase
                {
                    Name = request.Name
                };
                this.context.UseCases.Add(usecase);
                this.context.SaveChanges();
            }else
            {
                throw new UnproccessableEntityException(result.Errors);
            }
        }
    }
}
