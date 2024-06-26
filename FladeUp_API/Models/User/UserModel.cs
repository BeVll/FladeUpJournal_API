﻿using FladeUp_Api.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace FladeUp_API.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Image { get; set; }
        public string? IndetificateCode { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Sex { get; set; }
        public string National { get; set; }
        public string? Passport { get; set; }
        public bool IsLightTheme { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string Status { get; set; }
        public string? BankAccount { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }

        public string? MailCountry { get; set; }
        public string? MailCity { get; set; }
        public string? MailStreet { get; set; }
        public string? MailPostalCode { get; set; }

        public string? WorkExp { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
