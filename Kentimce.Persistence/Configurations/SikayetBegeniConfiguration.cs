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
    public class SikayetBegeniConfiguration : BaseEntityConfiguration<SikayetBegeni>
    {
        public override void Configure(EntityTypeBuilder<SikayetBegeni> builder)
        {
            base.Configure(builder);
            builder.ToTable("sikayetBegenileri");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Tarih).HasDefaultValueSql("now()");
            builder.HasIndex(x => new { x.SikayetId, x.KullaniciId }).IsUnique();
            builder.HasOne(x => x.sikayet)
                   .WithMany(x => x.begeniler)
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.kullanici)
                   .WithMany()
                   .HasForeignKey(x => x.KullaniciId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
