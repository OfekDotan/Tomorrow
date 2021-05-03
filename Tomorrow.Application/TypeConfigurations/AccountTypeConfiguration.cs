using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tomorrow.DomainModel;
using Tomorrow.DomainModel.Accounts;

namespace Tomorrow.Application.TypeConfigurations
{
	internal class AccountTypeConfiguration : IEntityTypeConfiguration<Account>
	{
		public void Configure(EntityTypeBuilder<Account> builder)
		{
			builder.HasKey(a => a.Id);
			builder
				.Property(a => a.Id)
				.HasConversion(id => id.ToGuid(), guid => new Identifier<Account>(guid));
		}
	}
}