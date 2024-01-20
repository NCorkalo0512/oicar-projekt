namespace FriendFinderAPI.Model.DTO
{
    public class ProfileDTO
    {
        public int Idprofile { get; set; }

        public string? Technology { get; set; }

        public string? ProjectDescription { get; set; }

        public bool? Enabled { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }
    }
}
