using CongDoanCoreApp.Data.Enums;
using CongDoanCoreApp.Data.Interfaces;
using CongDoanCoreApp.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CongDoanCoreApp.Data.Entities
{
    [Table("Languages")]
    public class Language : DomainEntity<string>, ISwitchable
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public string Resources { get; set; }
        public Status Status { get; set; }
    }
}