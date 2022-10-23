using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Models
{
    public class Messages
    {
        public Messages()
        {
            
            this.MessageTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public User UserSender { get; set; } = null!;
        public User Reciever { get; set; } = null!;


       
        public string message { get; set; } = null!;
        public DateTime MessageTime { get; set; }

    }
}
