using CongDoanCoreApp.Data.Enums;
using CongDoanCoreApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CongDoanCoreApp.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, ISwitchable, IDateTracking
    {
        public AppUser()
        {
        }

        public AppUser(Guid id, string fullName, string userName, string email, string phoneNumber, string avatar, Status status)
        {
            this.Id = id;
            this.FullName = fullName;
            this.UserName = userName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Avatar = avatar;
            this.Status = status;
        }

        public string FullName { get; set; }

        public DateTime? BirthDay { get; set; }

        public decimal Balance { get; set; }
        public string Avatar { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public Status Status { get; set; }
    }
}