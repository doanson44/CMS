using CMS.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Data.Configurations
{
    public class CategoryNewsConfiguration : IEntityTypeConfiguration<CategoryNews>
    {
        public void Configure(EntityTypeBuilder<CategoryNews> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Name }).IsUnique();
        }
    }
}
