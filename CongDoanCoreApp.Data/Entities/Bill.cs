using CongDoanCoreApp.Data.Enums;
using CongDoanCoreApp.Data.Interfaces;
using CongDoanCoreApp.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CongDoanCoreApp.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>, IDateTracking, ISwitchable
    {
        public Bill()
        {
        }

        public Bill(string customerName, string customerAddress, string customerMobile, string customerMessage,
                    BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid customerId)
        {
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
        }

        public Bill(int id, string customerName, string customerAddress, string customerMobile, string customerMessage,
           BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid customerId)
        {
            Id = id;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
        }

        [MaxLength(255)]
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(255)]
        public string CustomerAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { get; set; }

        [Required]
        [MaxLength(255)]
        public string CustomerMessage { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public BillStatus BillStatus { get; set; }

 
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<BillDetail> BillDetails { get; set; }

        [DefaultValue(Status.Active)]
        public Status Status { get; set; } = Status.Active;

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}