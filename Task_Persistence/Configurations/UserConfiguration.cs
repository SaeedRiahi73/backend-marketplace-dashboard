using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Domain.Entities;

namespace Task_Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasData(
                new
                {
                    Id = Guid.Parse("6f3a3e68-80f2-49af-bb08-2e8b2b71569a"), // آی‌دی ثابت
                    Username = "saeed",
                    Email = "saeed@gmail.com",
                    PasswordHash = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1f8c4e3e5a9b7d2d8b1c5d"
                }
                );
        }
    }
}
