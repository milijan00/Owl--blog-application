using BlogApp.Application.UseCases.Commands;
using BlogApp.DataAccess;
using BlogApp.DataAccess.Extensions;
using BlogApp.Domain;
using BlogApp.Implementations.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Implementations.UseCases.Ef.Command
{
    public class EfDeleteUseCaseComand : EfBase, IDeleteUseCaseCommand
    {
        public EfDeleteUseCaseComand(BlogAppDbContext context) : base(context)
        {
        }

        public int Id => 45;

        public string Name => "EfDeleteUseCaseCommand";

        public string Description => "";

        public bool AdminOnly => true;

        public void Execute(int request)
        {
            var useCase = this.context.UseCases.Find(request);
            if(useCase == null)
            {
                throw new EntityNotFoundException(nameof(UseCase), request);
            }
            this.context.Deactivate(useCase);
            this.context.SaveChanges();
        }
    }
}
