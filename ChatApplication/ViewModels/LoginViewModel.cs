using System.ComponentModel.DataAnnotations;

namespace ChatApplication.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        
        public string UserName { get; set; } = null!;
        [Required]
        
        public string password { get; set; } = null!;
    }
}
