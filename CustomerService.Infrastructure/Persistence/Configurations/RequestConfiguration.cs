using CustomerService.Domain.Request;
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

    public sealed class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.RequestId)
                .HasConversion(id => id.Value, value => RequestId.Create(value))
                .HasColumnName("RequestId")
                .ValueGeneratedNever();

            builder.Property(r => r.ReferenceNumber).IsRequired().HasMaxLength(50);

            builder.Property(r => r.CustomerId)
                .HasConversion(id => id.Value, value => UserId.Create(value))
                .HasColumnName("CustomerId");

            builder.Property(r => r.AgentId)
                .HasConversion(
                    id => id == null ? (Guid?)null : id.Value,
                    value => value == null ? null : UserId.Create(value.Value))
                .HasColumnName("AgentId");

            builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(r => r.Urgency).HasConversion<string>().HasMaxLength(50);
            builder.Property(r => r.Category).HasConversion<string>().HasMaxLength(50);

            builder.Property(r => r.Description).IsRequired().HasMaxLength(2000);

            builder.HasMany(typeof(Log), nameof(Request.Logs))
                .WithOne()
                .HasForeignKey("RequestId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(nameof(Request.Logs))
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
