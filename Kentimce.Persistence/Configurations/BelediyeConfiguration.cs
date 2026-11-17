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
    public class BelediyeConfiguration : BaseEntityConfiguration<Belediye>
    {
        public override void Configure(EntityTypeBuilder<Belediye> builder)
        {
            base.Configure(builder);
            builder.ToTable("belediyeler");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
           .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.Isim).IsRequired();
            builder.Property(x => x.Tip).HasMaxLength(50);
            builder.Property(x => x.Adres);

            builder.HasOne(x => x.il)
                   .WithMany(x => x.belediyeler)
                   .HasForeignKey(x => x.IlId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ilce)
                   .WithMany(x => x.belediyeler)
                   .HasForeignKey(x => x.IlceId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.sikayetler)
                   .WithOne(x => x.belediye)
                   .HasForeignKey(x => x.BelediyeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
