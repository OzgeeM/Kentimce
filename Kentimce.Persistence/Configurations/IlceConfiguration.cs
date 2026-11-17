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
    public class IlceConfiguration : BaseEntityConfiguration<Ilce>
    {
        public override void Configure(EntityTypeBuilder<Ilce> builder)
        {
            base.Configure(builder);
            builder.ToTable("ilceler");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
            .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Isim).IsRequired();

            builder.HasOne(x => x.il)
                   .WithMany(x => x.ilceler)
                   .HasForeignKey(x => x.IlId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.belediyeler)
                   .WithOne(x => x.ilce)
                   .HasForeignKey(x => x.IlceId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
