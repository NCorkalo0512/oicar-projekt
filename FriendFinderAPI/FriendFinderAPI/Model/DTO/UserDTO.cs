namespace FriendFinderAPI.Model.DTO
{
    public class UserDTO
    {
        public int Iduser { get; set; }
        public int IdUserProfile { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public bool Verification { get; set; }

        public string UserType { get; set; }

        public string Token { get; set; }

        public string? Technology { get; set; }

        public string? ProjectDescription { get; set; }

        public bool? Enabled { get; set; }

      
    }
}
