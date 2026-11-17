using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Persistence.Configurations
{
    using Kentimce.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NetTopologySuite.Geometries;

    public class SikayetConfiguration : BaseEntityConfiguration<Sikayet>
    {
        public override void Configure(EntityTypeBuilder<Sikayet> builder)
        {
            base.Configure(builder);
            builder.ToTable("sikayetler");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
           .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Baslik).IsRequired();
            builder.Property(x => x.Aciklama).IsRequired();
            builder.Property(x => x.Adres);
            builder.Property(x => x.Durum).IsRequired().HasDefaultValue("Yeni");
            builder.Property(x => x.OnInceleme).HasDefaultValue(true);
            builder.Property(x => x.OlusturmaTarihi).HasDefaultValueSql("now()");
            builder.Property(x => x.GuncellemeTarihi);

            // geometry
            //builder.Property(x => x.Koordinat).HasColumnType("geometry (Point,4326)");

            builder.HasOne(x => x.kullanici)
                   .WithMany(x => x.sikayetler)
                   .HasForeignKey(x => x.KullaniciId)
                   .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.belediye)
                   .WithMany(x => x.sikayetler)
                   .HasForeignKey(x => x.BelediyeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ekler)
                   .WithOne(x => x.sikayet)
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.durumGecmisleri)
                   .WithOne(x => x.sikayet)
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.yorumlar)
                   .WithOne(x => x.sikayet)
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.begeniler)
                   .WithOne(x => x.sikayet)
                   .HasForeignKey(x => x.SikayetId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
