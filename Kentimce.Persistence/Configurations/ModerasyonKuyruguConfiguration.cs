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
    public class ModerasyonKuyruguConfiguration : BaseEntityConfiguration<ModerasyonKuyrugu>
    {
        public override void Configure(EntityTypeBuilder<ModerasyonKuyrugu> builder)
        {
            base.Configure(builder);
            builder.ToTable("moderasyonKuyrugu");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Neden);
            builder.Property(x => x.Durum);
            builder.Property(x => x.IslemTarihi);
            builder.HasOne(x => x.sikayet)
                   .WithMany()
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.islemiYapanKullanici)
                   .WithMany()
                   .HasForeignKey(x => x.IslemiYapan)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
