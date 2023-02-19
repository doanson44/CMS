using CMS.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Data.Configurations
{
    public class DetailNewsConfiguration : IEntityTypeConfiguration<DetailNews>
    {
        public void Configure(EntityTypeBuilder<DetailNews> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ViewNews)
                .WithOne(x => x.DetailNews)
                .HasForeignKey<ViewNews>(x => x.DetailNewsId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
