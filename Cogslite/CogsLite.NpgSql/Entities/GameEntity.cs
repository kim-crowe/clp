using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CogsLite.NpgSql.Entities
{

    public class GameEntity : NamedEntity, IEntityTypeConfiguration<GameEntity>
    {
        public Guid OwnerId { get; set; }
        public UserEntity Owner { get; set; }

        public List<CardEntity> Cards { get; set; }

        public List<CardTypeEntity> CardTypes { get; set; }

        public void Configure(EntityTypeBuilder<GameEntity> builder)
        {
            builder.ToTable("game", "cogs");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name")
                .HasColumnType($"character varying(100)");

            builder.Property(g => g.OwnerId)
                .HasColumnName("owner_id")
                .IsRequired();

            builder
                .HasOne(g => g.Owner)
                .WithMany(usr => usr.Games)
                .HasForeignKey(g => g.OwnerId)
                .HasConstraintName("fk_cogs_game_owner");
        }
    }
}