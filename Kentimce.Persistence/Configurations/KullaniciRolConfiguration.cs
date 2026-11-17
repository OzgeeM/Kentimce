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
    public class KullaniciRolConfiguration : BaseEntityConfiguration<KullaniciRol>
    {
        public override void Configure(EntityTypeBuilder<KullaniciRol> builder)
        {
            base.Configure(builder);
            builder.ToTable("kullaniciRolleri");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.HasOne(x => x.kullanici)
                   .WithMany(x => x.roller)
                   .HasForeignKey(x => x.KullaniciId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.rol)
                   .WithMany(x => x.kullaniciRolleri)
                   .HasForeignKey(x => x.RolId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
