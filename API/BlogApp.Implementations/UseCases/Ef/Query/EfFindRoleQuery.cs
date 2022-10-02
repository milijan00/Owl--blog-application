using BlogApp.Application.Dto;
using BlogApp.Application.UseCases.Queries;
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
    public class EfFindRoleQuery : EfBase, IFindRoleQuery
    {
        private AutoMapper.IMapper _mapper;
        public EfFindRoleQuery(BlogApp.DataAccess.BlogAppDbContext context,AutoMapper.IMapper mapper ) : base(context) { _mapper = mapper; }
        public string Name => "Find a specific role";

        public string Description => "Find a specific role via id.";

        public int Id => 30;

        public bool AdminOnly => false;

        public RoleDto Execute(int id)
        {
            var role =  this.context.Roles.Include(x => x.UseCases).FirstOrDefault(x => x.Id == id && x.IsActive);
            if(role == null)
            {
                throw new EntityNotFoundException(nameof(Role), id);
            }
            return new RoleWithUsecasesDto {
                Id = role.Id,
                Name = role.Name,
                  Usecases = role.UseCases.Select(x => new RoleUsecaseDto
                  {
                      RoleId = x.RoleId,
                      UsecaseId = x.UseCaseId
                  }).ToList()
            };
        }
    }
}
