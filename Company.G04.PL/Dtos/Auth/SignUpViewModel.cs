﻿using System.ComponentModel.DataAnnotations;

namespace Company.G04.PL.Dtos.Auth
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "UserName is Required !!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName is Required !!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required !!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email !!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirmed password does not match password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "IsAgree is Required !!")]
        public bool IsAgreed { get; set; }

    }
}
