using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_Domain.Entities;

namespace Task_Persistence.Configurations;

public sealed class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(refreshToken => refreshToken.Id);

        builder.Property(refreshToken => refreshToken.TokenHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasIndex(refreshToken => refreshToken.TokenHash)
            .IsUnique();

        builder.Property(refreshToken => refreshToken.CreatedAt)
            .IsRequired();

        builder.Property(refreshToken => refreshToken.ExpiresAt)
            .IsRequired();

        builder.Property(refreshToken => refreshToken.IsPersistent)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(refreshToken => refreshToken.RevokedAt)
            .IsRequired(false);

        builder.Property(refreshToken => refreshToken.ReplacedByTokenId)
            .IsRequired(false);

        builder.HasIndex(refreshToken => refreshToken.UserId);

        builder.HasIndex(refreshToken => refreshToken.ExpiresAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<RefreshToken>()
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.ReplacedByTokenId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
