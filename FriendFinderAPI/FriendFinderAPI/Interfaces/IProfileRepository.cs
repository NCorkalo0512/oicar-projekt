using FriendFinderAPI.Model;
using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Model.DTO;

namespace FriendFinderAPI.Interfaces
{
    public interface IProfileRepository
    {

        ProfileDTO CreateProfile(CreateProfileCommand createProfile);
        bool UpdateProfile(UpdateProfileCommand updateProfile);/* uspjesno ako je prosao update ili nije prosao*/

        void ToggleProfile(int idProfile);
            

        ProfileDTO GetProfile(int idProfile);
        IEnumerable<ProfileDTO> GetAllProfiles();
    }
}
