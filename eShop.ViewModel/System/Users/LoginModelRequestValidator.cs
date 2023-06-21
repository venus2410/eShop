using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.System.Users
{
    public class LoginModelRequestValidator : AbstractValidator<LoginModelRequest>
    {
        public LoginModelRequestValidator()
        {
            RuleFor(m=>m.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(m => m.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password is at least 8 characters");
        }
    }
}
