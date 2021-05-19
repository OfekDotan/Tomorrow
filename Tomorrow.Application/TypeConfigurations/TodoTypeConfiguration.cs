using System.Collections.Generic;
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

			builder
				.HasMany(t => t.accountsThatCanEdit)
				.WithMany(a => a.EditableTodos)
				.UsingEntity<Dictionary<string, object>>("TodoEditPermissions", j =>
				  {
					  return j
					   .HasOne<Account>()
					   .WithMany()
					   .OnDelete(DeleteBehavior.ClientCascade);
				  }, j =>
				  {
					  return j
					  .HasOne<Todo>()
					  .WithMany()
					  .OnDelete(DeleteBehavior.ClientCascade);
				  });

			builder
				.HasMany(t => t.accountsThatCanView)
				.WithMany(a => a.ViewableTodos)
				.UsingEntity<Dictionary<string, object>>("TodoViewPermissions", j =>
				{
					return j
					 .HasOne<Account>()
					 .WithMany()
					 .OnDelete(DeleteBehavior.ClientCascade);
				}, j =>
				{
					return j
					.HasOne<Todo>()
					.WithMany()
					.OnDelete(DeleteBehavior.ClientCascade);
				});
		}
	}
}