using CMS.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Data.Configurations;

public class ViewNewsConfiguration : IEntityTypeConfiguration<ViewNews>
{
    public void Configure(EntityTypeBuilder<ViewNews> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
