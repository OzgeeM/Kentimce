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
    public class KullaniciConfiguration : BaseEntityConfiguration<Kullanici>
    {
        public override void Configure(EntityTypeBuilder<Kullanici> builder)
        {
            base.Configure(builder);
            builder.ToTable("kullanicilar");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Email).HasMaxLength(320);
            builder.HasIndex(x => x.Email).IsUnique().HasFilter(null); // Postgres: unique email (nullable ok)
            builder.Property(x => x.Telefon).HasMaxLength(20);
            builder.Property(x => x.Ad);
            builder.Property(x => x.Soyad);
            builder.Property(x => x.ParolaHash);
            builder.Property(x => x.OlusturmaTarihi).HasDefaultValueSql("now()");

            builder.HasMany(x => x.sikayetler)
                   .WithOne(x => x.kullanici)
                   .HasForeignKey(x => x.KullaniciId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.roller)
                   .WithOne(x => x.kullanici)
                   .HasForeignKey(x => x.KullaniciId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(k => k.Belediye)
               .WithMany(b => b.Personeller)
               .HasForeignKey(k => k.BelediyeId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict); 
        }
    }

}
