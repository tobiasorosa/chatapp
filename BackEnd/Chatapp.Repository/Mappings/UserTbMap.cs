using Chatapp.Application.Users.CommandSide.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatapp.Repository.Mappings
{
    internal class UserTbMap : IEntityTypeConfiguration<User>
    {
        private const string _tableName = "user_tb";
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(_tableName);

            builder.HasKey(e => e.Id)
                .HasName($"pk_{_tableName}");

            builder.Property(e => e.Id)
                .UseIdentityAlwaysColumn();

            builder.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Birthdate)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(60)
                .IsRequired();

            builder.Property<DateTime>("InsertionDate")
                .HasDefaultValueSql("now()");

            builder.Property<DateTime?>("ModificationDate");

            builder.Ignore(e => e.Events);
        }
    }
}
