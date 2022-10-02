using BlogApp.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DataAccess.Configuration
{
    public class UseCasesConfiguration : EntityConfiguration<UseCase>
    {
        protected override void ConfigureChildRules(EntityTypeBuilder<UseCase> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(60).IsRequired(true);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.UseCase)
                .HasForeignKey(x => x.UseCaseId);
        }
    }
}
