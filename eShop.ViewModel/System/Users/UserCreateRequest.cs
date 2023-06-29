using eShop.ViewModel.Catalog.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShop.ViewModel.System.Users
{
    public class UserCreateRequest
    {
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
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Required]
        [Compare(nameof(Password),ErrorMessage = "Xác nhận mật khẩu không khớp với mật khẩu")]
        public string ConfirmPassword { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
