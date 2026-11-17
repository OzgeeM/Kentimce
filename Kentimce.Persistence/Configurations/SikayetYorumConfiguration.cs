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
    public class SikayetYorumConfiguration : BaseEntityConfiguration<SikayetYorum>
    {
        public override void Configure(EntityTypeBuilder<SikayetYorum> builder)
        {
            base.Configure(builder);
            builder.ToTable("sikayetYorumlari");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Yorum).IsRequired();
            builder.HasOne(x => x.sikayet)
                   .WithMany(x => x.yorumlar)
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.kullanici)
                   .WithMany()
                   .HasForeignKey(x => x.KullaniciId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
