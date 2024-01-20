using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model;
using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace FriendFinderAPI.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        public FriendFinderDbContext _context { get; set; }
        public ProfileRepository(FriendFinderDbContext context)
        {
            _context = context;
        }
        public ProfileDTO CreateProfile(CreateProfileCommand createProfile)
        {
            if (string.IsNullOrWhiteSpace(createProfile.Technology))
                throw new ApplicationException("Technology is required");

            if (string.IsNullOrWhiteSpace(createProfile.ProjectDescription))
                throw new ApplicationException("Project Description is required");

            if (createProfile.UserId == null)
                throw new ApplicationException("User id is required");

            var user = _context.Users.Find(createProfile.UserId);

            if (user == null)
                throw new ApplicationException("User not found, check if id is correct");

            var profile = new Profile()
            {
                Technology = createProfile.Technology,
                ProjectDescription = createProfile.ProjectDescription,
                UserId = createProfile.UserId,
                Enabled = true
            };
            _context.Profiles.Add(profile);
            _context.SaveChanges();

            var newProfile = new ProfileDTO()
            {
                Idprofile = profile.Idprofile,
                Technology = profile.Technology,
                ProjectDescription = profile.ProjectDescription,
                UserId = profile.UserId,
                Enabled = true,
                UserName = user.FirstName + ' ' + user.LastName
            };
            return newProfile;
        }

        public void ToggleProfile(int idUser)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.UserId == idUser);

            if (profile == null)
                throw new ApplicationException("Profile not found");

            profile.Enabled = !profile.Enabled;

            _context.Profiles.Update(profile);
            _context.SaveChanges();
        }

        public bool UpdateProfile(UpdateProfileCommand updateProfile)
        {
            var profile = _context.Profiles.Find(updateProfile.Idprofile);

            if (profile == null)
                throw new ApplicationException("Profile not found");


            if (!string.IsNullOrWhiteSpace(updateProfile.ProjectDescription))
                profile.ProjectDescription = updateProfile.ProjectDescription;

            if (!string.IsNullOrWhiteSpace(updateProfile.Technology))
                profile.Technology = updateProfile.Technology;


            _context.Profiles.Update(profile);
            _context.SaveChanges();

            return true;
        }

        public ProfileDTO GetProfile(int idProfile)
        {
            var profile = _context.Profiles.Find(idProfile);

            if (profile == null)
                throw new ApplicationException("Profile not found");

            var user = _context.Users.Find(profile.UserId);
            var newProfile = new ProfileDTO()
            {
                Idprofile = profile.Idprofile,
                Technology = profile.Technology,
                ProjectDescription = profile.ProjectDescription,
                UserId = profile.UserId,
                Enabled = true,
                UserName = user.FirstName + ' ' + user.LastName
            };
            return newProfile;
           
        }

        public IEnumerable<ProfileDTO> GetAllProfiles()
        {
            var profiles = _context.Profiles.ToList();
            var profilesDTOs = new List<ProfileDTO>();
            foreach (var profile in profiles)
            {
                var user = _context.Users.Find(profile.UserId);
                var newProfile = new ProfileDTO()
                {
                    Idprofile = profile.Idprofile,
                    Technology = profile.Technology,
                    ProjectDescription = profile.ProjectDescription,
                    UserId = profile.UserId,
                    Enabled = true,
                    UserName = user.FirstName + ' ' + user.LastName
                };
                profilesDTOs.Add(newProfile);
            }
            return profilesDTOs;
        }
    }
}
