using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Accounts;
using Tomorrow.DomainModel.Groups;

namespace Tomorrow.Application.TypeConfigurations
{
	internal class GroupTypeConfiguration : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.HasKey(t => t.Id);
			builder.Property(t => t.Id).HasConversion(id => id.ToGuid(), guid => new Identifier<Group>(guid));

			builder.HasOne<Account>().WithMany().HasForeignKey(g => g.OwnerId);

			builder
				.Property(t => t.Name)
				.HasMaxLength(256);
		}
	}
}