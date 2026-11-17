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
    public class SikayetEkConfiguration : BaseEntityConfiguration<SikayetEk>
    {
        public override void Configure(EntityTypeBuilder<SikayetEk> builder)
        {
            base.Configure(builder);
            builder.ToTable("sikayetEkleri");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
          .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.DosyaIsmi).IsRequired(false);
            builder.Property(x => x.DosyaUrl).IsRequired();
            builder.Property(x => x.MimeType).HasMaxLength(200);
            builder.Property(x => x.YuklemeTarihi).HasDefaultValueSql("now()");
        }
    }

}
