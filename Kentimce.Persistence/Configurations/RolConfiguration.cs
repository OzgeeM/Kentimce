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
    public class RolConfiguration : BaseEntityConfiguration<Rol>
    {
        public override void Configure(EntityTypeBuilder<Rol> builder)
        {
            base.Configure(builder);
            builder.ToTable("roller");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Isim).IsRequired();
            builder.HasIndex(x => x.Isim).IsUnique();
        }
    }

}
