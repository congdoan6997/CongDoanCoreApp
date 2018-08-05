﻿using CongDoanCoreApp.Data.EF.Extensions;
using CongDoanCoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongDoanCoreApp.Data.EF.Configurations
{
    public class ContactDetailConfiguration : DbEntityConfiguration<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
        }
    }
}