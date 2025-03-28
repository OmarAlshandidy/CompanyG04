using System.ComponentModel.DataAnnotations;

namespace Company.G04.PL.Dtos.Auth
{
    public class ResetPasswordDto
    {

        [Required(ErrorMessage = "Password is Required !!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirmed password does not match password")]
        public string ConfirmPassword { get; set; }
    }
}
