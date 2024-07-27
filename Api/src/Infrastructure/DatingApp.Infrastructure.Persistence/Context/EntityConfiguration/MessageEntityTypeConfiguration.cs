using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Persistence.Context.EntityConfiguration
{
    public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {


            builder.HasOne(m => m.Receiver)
                .WithMany(u => u.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSend)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
