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
    public class SikayetDurumConfiguration : BaseEntityConfiguration<SikayetDurum>
    {
        public override void Configure(EntityTypeBuilder<SikayetDurum> builder)
        {
            base.Configure(builder);
            builder.ToTable("sikayetDurumlari");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Durum);
            builder.Property(x => x.Aciklama);
            builder.Property(x => x.IslemTarihi).HasDefaultValueSql("now()");
            builder.HasOne(x => x.sikayet)
                   .WithMany(x => x.durumGecmisleri)
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.IslemYapanKullanici)
                   .WithMany()
                   .HasForeignKey(x => x.IslemYapanId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
