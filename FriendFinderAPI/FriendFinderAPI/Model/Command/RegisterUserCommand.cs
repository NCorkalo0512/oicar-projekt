using System.ComponentModel.DataAnnotations;

namespace FriendFinderAPI.Model.Command
{
    public class RegisterUserCommand
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Technology { get; set; }
        [Required]
       public string ProjectDescription { get; set; }
    }
}
