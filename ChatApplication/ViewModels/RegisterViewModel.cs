using System.ComponentModel.DataAnnotations;

namespace ChatApplication.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; } = null!;
        [Required]
        [Compare("password", ErrorMessage = "Password and confirm Password Doesn't Match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
