using FriendFinderAPI.Interfaces;
using FriendFinderAPI.Model;
using FriendFinderAPI.Model.Command;
using FriendFinderAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace FriendFinderAPI.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        public FriendFinderDbContext _context { get; set; }
        public MatchRepository(FriendFinderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MatchDTO>> GetAllMatchedForUser(int userId)
        {
            var allMatched = await _context.Matchings.Where(match => (match.UserId1 == userId || match.UserId2 == userId) && match.Swipe == true).ToListAsync();
            var matchesDTOs = new List<MatchDTO>();
            foreach (var match in allMatched)
            {
                var userSwiper = _context.Users.Include(u => u.Profiles).FirstOrDefault(user => user.Iduser == match.UserId1 && user.Iduser != userId);
                if (userSwiper != null)
                {
                    if (userSwiper.UserTypeId != 1)
                    {
                        var newMatch = new MatchDTO()
                        {
                            Idmatching = match.Idmatching,
                            Technology = userSwiper.Profiles.FirstOrDefault()?.Technology,
                            ProjectDescription = userSwiper.Profiles.FirstOrDefault()?.ProjectDescription,
                            FirstName = userSwiper.FirstName,
                            LastName = userSwiper.LastName,
                            Swipe = match.Swipe,
                            DateTime = match.DateTime,
                            UserId = userSwiper.Iduser
                        };
                        matchesDTOs.Add(newMatch);
                    }
                }

                var userSwipped = _context.Users.Include(u => u.Profiles).FirstOrDefault(user => user.Iduser == match.UserId2 && user.Iduser != userId);
                if (userSwipped != null)
                {
                    if (userSwipped.UserTypeId != 1)
                    {
                        var newMatch = new MatchDTO()
                        {
                            Idmatching = match.Idmatching,
                            Technology = userSwipped.Profiles.FirstOrDefault()?.Technology,
                            ProjectDescription = userSwipped.Profiles.FirstOrDefault()?.ProjectDescription,
                            FirstName = userSwipped.FirstName,
                            LastName = userSwipped.LastName,
                            Swipe = match.Swipe,
                            DateTime = match.DateTime,
                            UserId = userSwipped.Iduser
                        };
                        matchesDTOs.Add(newMatch);
                    }
                }

            }
            return matchesDTOs;
        }

        public async Task<IEnumerable<MatchDTO>> GetAllNotMatchedForUser(int userId)
        {
            var allMatched1 = await _context.Matchings.Where(match => match.UserId1 == userId && match.Swipe == true).Select(x => x.UserId2).ToListAsync();
            var allMatched2 = await _context.Matchings.Where(match => match.UserId2 == userId && match.Swipe == true).Select(x => x.UserId1).ToListAsync();
            var matchesUserIds = allMatched1.Concat(allMatched2);

            var allUserIds = _context.Users.Where(user => user.Iduser != userId).Select(x => x.Iduser).ToList();
            foreach (var matchUserId in matchesUserIds)
            {
                allUserIds.Remove((int)matchUserId);
            }
            var matchesDTOs = new List<MatchDTO>();
            foreach (var allUserId in allUserIds)
            {
                var user = _context.Users.Include(u => u.Profiles).FirstOrDefault(user => user.Iduser == allUserId);
                if (user.UserTypeId != 1)
                {
                    var newMatch = new MatchDTO()
                    {
                        Technology = user.Profiles.FirstOrDefault()?.Technology,
                        ProjectDescription = user.Profiles.FirstOrDefault()?.ProjectDescription,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Swipe = false,
                        UserId = user.Iduser
                    };
                    matchesDTOs.Add(newMatch);
                }
            }
            return matchesDTOs;
        }

        public void SwipeForUser(MatchCommand match)
        {
            try
            {
            var swippedUser = int.Parse(match.SwippedUser);
            var swiperUser = int.Parse(match.SwiperUser);
            var user1 = _context.Users.FirstOrDefault(user => user.Iduser == swippedUser);
            var user2 = _context.Users.FirstOrDefault(user => user.Iduser == swiperUser);
            if (user1 == null && user2 == null)
            {
                throw new ArgumentNullException("Korisnici ne postoje!");
            }

            //Ukoliko vec postoji match od prije, updejtaj datum metchanja i promijeni swipe vrijednost u novu
            var matchExisting = _context.Matchings.Where(user => user.UserId1 == swippedUser && user.UserId2 == swiperUser).FirstOrDefault();
            if (matchExisting != null)
            {
                matchExisting.DateTime = DateTime.Now;
                matchExisting.Swipe = match.Swipe;
            }

            //Isto kao i gore, samo u zamjenjenim ulogama swiper i swipped usera
            var matchExisting2 = _context.Matchings.Where(user => user.UserId1 == swiperUser && user.UserId2 == swippedUser).FirstOrDefault();
            if (matchExisting2 != null)
            {
                matchExisting2.DateTime = DateTime.Now;
                matchExisting2.Swipe = match.Swipe;
            }

            //Ukoliko ne postoje matchevi od prije izmedu te dvije osobe, dodaj novi match
            if (matchExisting == null && matchExisting2 == null)
            {
                var matched = new Matching()
                {
                    UserId1 = swiperUser,
                    UserId2 = swippedUser,
                    DateTime = DateTime.UtcNow,
                    Swipe = match.Swipe
                };

                _context.Matchings.Add(matched);
            }


            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }
    }
}
