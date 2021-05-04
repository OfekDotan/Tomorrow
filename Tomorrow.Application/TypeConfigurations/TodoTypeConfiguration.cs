using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.DomainModel.Groups;
using Tomorrow.DomainModel.Todos;

namespace Tomorrow.Application.TypeConfigurations
{
	internal class TodoTypeConfiguration : IEntityTypeConfiguration<Todo>
	{
		public void Configure(EntityTypeBuilder<Todo> builder)
		{
			builder.HasKey(t => t.Id);
			builder.Property(t => t.Id).HasConversion(id => id.ToGuid(), guid => new Identifier<Todo>(guid));

			builder.OwnsOne(t => t.Priority, priorityBuilder =>
			{
				priorityBuilder.Property<int>("priority").HasColumnName("Priority");
			});

			builder
				.HasOne<Group>()
				.WithMany()
				.HasForeignKey(t => t.GroupId).OnDelete(DeleteBehavior.ClientCascade);
			builder
				.HasOne<Account>()
				.WithMany()
				.HasForeignKey(t => t.OwnerId);
			builder
				.Property(t => t.Name)
				.HasMaxLength(256);
		}
	}
}