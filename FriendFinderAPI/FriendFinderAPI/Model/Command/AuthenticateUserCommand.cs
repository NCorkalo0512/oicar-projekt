using System.ComponentModel.DataAnnotations;

namespace FriendFinderAPI.Model.Command
{
    public class AuthenticateUserCommand
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
