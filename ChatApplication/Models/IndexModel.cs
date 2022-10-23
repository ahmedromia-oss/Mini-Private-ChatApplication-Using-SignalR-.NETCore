using ChatApplication.ViewModels;

namespace ChatApplication.Models
{
    public class IndexModel
    {

        public List<Messages> messages = null!;
        public List<User> users = null!;
        public UpdateUser updateUser;
    }
}
