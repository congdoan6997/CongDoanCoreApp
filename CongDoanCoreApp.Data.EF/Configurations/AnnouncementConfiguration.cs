using CongDoanCoreApp.Data.EF.Extensions;
using CongDoanCoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongDoanCoreApp.Data.EF.Configurations
{
    public class AnnouncementConfiguration : DbEntityConfiguration<Announcement>
    {
        public override void Configure(EntityTypeBuilder<Announcement> entity)
        {

            entity.Property(x => x.Id).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
        }
    }
}