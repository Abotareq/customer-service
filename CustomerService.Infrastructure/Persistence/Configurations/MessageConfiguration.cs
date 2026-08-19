using CustomerService.Domain.Message;
using CustomerService.Domain.Message.ValueObjects;
using CustomerService.Domain.Request.ValueObjects;
using CustomerService.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Persistence.Configurations
{
    public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageId)
                .HasConversion(id => id.Value, value => MessageId.Create(value))
                .HasColumnName("MessageId")
                .ValueGeneratedNever();

            builder.Property(m => m.RequestId)
                .HasConversion(id => id.Value, value => RequestId.Create(value))
                .HasColumnName("RequestId");

            builder.Property(m => m.SenderId)
                .HasConversion(id => id.Value, value => UserId.Create(value))
                .HasColumnName("SenderId");

            builder.Property(m => m.Content).IsRequired().HasMaxLength(2000);

            builder.HasIndex(m => m.RequestId);
        }
    }
}
