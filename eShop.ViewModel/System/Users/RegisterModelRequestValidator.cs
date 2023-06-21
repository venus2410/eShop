using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModel.System.Users
{
    public class RegisterModelRequestValidator : AbstractValidator<RegisterModelRequest>
    {
        public RegisterModelRequestValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("First name is required")
                .MaximumLength(200).WithMessage("First name is maximum 200 characters");

            RuleFor(m => m.LastName).NotEmpty().WithMessage("Last name is required")
                .MaximumLength(200).WithMessage("Last name is maximum 200 characters");

            RuleFor(m => m.Dob)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Day of birth can not be in future");

            RuleFor(m => m.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("The input is not an valid email address");

            RuleFor(m => m.PhoneNumber).NotEmpty().WithMessage("Phone number is required");

            RuleFor(m => m.UserName).NotEmpty().WithMessage("User name is required");

            RuleFor(m => m.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password is at least 8 characters");

            RuleFor(m => m.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required")
                .Equal(n => n.Password).WithMessage("Confirm password is not matched the password");

        }
    }
}
