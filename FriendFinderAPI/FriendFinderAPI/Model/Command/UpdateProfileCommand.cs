namespace FriendFinderAPI.Model.Command
{
    public class UpdateProfileCommand
    {
        public int Idprofile { get; set; }
        public string? Technology { get; set; }

        public string? ProjectDescription { get; set; }
    }
}
