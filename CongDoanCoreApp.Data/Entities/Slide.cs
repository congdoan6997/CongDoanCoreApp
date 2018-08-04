using CongDoanCoreApp.Data.Enums;
using CongDoanCoreApp.Data.Interfaces;
using CongDoanCoreApp.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CongDoanCoreApp.Data.Entities
{
    [Table("Slides")]
    public class Slide : DomainEntity<int>, ISwitchable
    {
        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        [Required]
        public string Image { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        public int? DisplayOrder { get; set; }

        public string Content { get; set; }
        public Status Status { get; set; }

        [StringLength(25)]
        [Required]
        public string GroupAlias { get; set; }
    }
}