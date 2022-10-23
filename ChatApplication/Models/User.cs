using Microsoft.AspNetCore.Identity;

namespace ChatApplication.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            this.connrctionId = "";
        }
        public string connrctionId { get; set; } = null!;
        public string UserPhoto { get; set; } = "";
    }
}
