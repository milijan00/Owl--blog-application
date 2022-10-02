using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Queries;
using BlogApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Query
{
    public class EfGetUseCasesQuery : EfBase, IGetUseCasesQuery
    {
        public EfGetUseCasesQuery(BlogAppDbContext context) : base(context)
        {
        }

        public int Id => 42;

        public string Name => "EfGetUseCasesQuery";

        public string Description => "";

        public bool AdminOnly => true;

        public IEnumerable<UseCaseDto> Execute()
        {
            return this.context.UseCases.Where(x => x.IsActive).Select(x => new UseCaseDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
