using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Application.UseCases.Commands;
using BlogApp.DataAccess;
using BlogApp.DataAccess.Extensions;
using BlogApp.Domain;

namespace BlogApp.Implementations.UseCases.Ef.Command
{
    public class EfDeleteRoleCommand : EfBase, IDeleteRoleCommand
    {
        public EfDeleteRoleCommand(BlogAppDbContext context) : base(context)
        {

        }
        public string Name => "EfDeleteRole";

        public string Description => "Delete a role with given id.";

        public int Id => 16;

        public bool AdminOnly => true;

        public void Execute(int id) {
            var role = this.context.Roles.Find(id);
            this.context.Deactivate<Role>(role.Id);
            this.context.SaveChanges();
        }
    }
}
