using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Queries;
using BlogApp.DataAccess;
using BlogApp.Domain;
using BlogApp.Implementations.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Query
{
    public class EfFindUseCaseQuery : EfBase, IFindUseCaseQuery
    {
        public EfFindUseCaseQuery(BlogAppDbContext context) : base(context)
        {
        }

        public int Id => 43;

        public string Name => "EfFindUseCaseQuery";

        public string Description => "";

        public bool AdminOnly => true;

        public UseCaseDto Execute(int request)
        {
            var useCase = this.context.UseCases.Include(x => x.Roles).ThenInclude(x => x.Role).FirstOrDefault(x => x.Id == request);
            if (useCase == null)
            {
                throw new EntityNotFoundException(nameof(UseCase), request);
            }
            return new UseCaseDto 
            {
                Id = useCase.Id,
                Name = useCase.Name
            };
        }
    }
}
