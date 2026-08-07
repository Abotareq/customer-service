using CustomerService.Domain.Request.Entites;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Persistence.Configurations
{
    public sealed class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.LogId)
                .HasConversion(id => id.Value, value => LogId.Create(value))
                .HasColumnName("LogId")
                .ValueGeneratedNever();

            builder.Property(l => l.FieldChanged).HasConversion<string>().HasMaxLength(50);
            builder.Property(l => l.OldValue).IsRequired().HasMaxLength(500);
            builder.Property(l => l.NewValue).IsRequired().HasMaxLength(500);

            builder.Property(l => l.ChangedByUserId)
                .HasConversion(
                    id => id == null ? (Guid?)null : id.Value,
                    value => value == null ? null : UserId.Create(value.Value))
                .HasColumnName("ChangedByUserId");

            builder.Property(l => l.Description).HasMaxLength(1000);
        }
    }
}
