using Kentimce.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Persistence.Configurations
{
    public class IlConfiguration : BaseEntityConfiguration<Il>
    {
        public override void Configure(EntityTypeBuilder<Il> builder)
        {
            base.Configure(builder);
            builder.ToTable("iller");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
            .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Isim).IsRequired();

            builder.HasMany(x => x.ilceler)
                   .WithOne(x => x.il)
                   .HasForeignKey(x => x.IlId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.belediyeler)
                  .WithOne(x => x.il)
                  .HasForeignKey(x => x.IlId)
                  .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
