using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CogsLite.NpgSql.Entities
{
    public class CardTypeEntity : NamedEntity, IEntityTypeConfiguration<CardTypeEntity>
    {
        public List<CardEntity> Cards { get; set; }

        public void Configure(EntityTypeBuilder<CardTypeEntity> builder)
        {
            builder.ToTable("card_type", "cogs");
            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Id).IsRequired();

            builder.Property(ct => ct.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .HasColumnType($"character varying(100)");
            
        }
    }
}