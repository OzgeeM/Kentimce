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
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.eklemeTarihi).HasColumnName("eklemeTarihi");
            builder.Property(x => x.ekleyenId).HasColumnName("ekleyenId");
            builder.Property(x => x.guncellemeTarihi).HasColumnName("guncellemeTarihi");
            builder.Property(x => x.guncelleyenId).HasColumnName("guncelleyenId");
            builder.Property(x => x.ip).HasColumnName("ip").HasMaxLength(50);
            builder.Property(x => x.silindi).HasColumnName("silindi").HasDefaultValue(false);
        }
    }

}
