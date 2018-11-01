using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CogsLite.NpgSql.Entities
{
    public class CardEntity : NamedEntity, IEntityTypeConfiguration<CardEntity>
    {
        public Guid TypeId { get; set; }
        public CardTypeEntity Type { get; set; }

        public Guid GameId { get; set; }
        public GameEntity Game { get; set; }

        public void Configure(EntityTypeBuilder<CardEntity> builder)
        {
            builder.ToTable("card", "cogs");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired();

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .HasColumnType($"character varying(100)");

            builder.Property(c => c.GameId);
            builder
                .HasOne(c => c.Game)
                .WithMany(g => g.Cards)
                .HasForeignKey(c => c.GameId)
                .HasConstraintName("fk_cogs_card_game");

            builder.Property(c => c.TypeId);
            builder
                .HasOne(c => c.Type)
                .WithMany(t => t.Cards)
                .HasForeignKey(c => c.TypeId)
                .HasConstraintName("fk_cogs_card_type");
        }
    }
}