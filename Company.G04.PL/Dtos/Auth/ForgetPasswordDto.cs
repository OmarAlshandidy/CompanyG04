using System.ComponentModel.DataAnnotations;

namespace Company.G04.PL.Dtos.Auth
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email !!")]
        public string Email { get; set; }
    }
}

