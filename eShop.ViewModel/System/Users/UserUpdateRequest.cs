using eShop.ViewModel.Catalog.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace eShop.ViewModel.System.Users
{
    public class UserUpdateRequest
    {
        public Guid Id { get; set; }
        [Display(Name = "Tên")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email")]
        [Remote(action: "IsValidEmail", controller: "User", AdditionalFields =nameof(Id),ErrorMessage = "Email has already used")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
