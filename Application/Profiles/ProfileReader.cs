using Application.Errors;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Persistence;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public ProfileReader(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Profile> ReadProfile(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound,
                    new { User = "Not found" });
            }

            var currentUser = await _context.Users.SingleOrDefaultAsync(u =>
                u.UserName == _userAccessor.GetCurrentUsername());

            var profile = new Profile
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                Image = user.MainPhotoUrl,
                Photos = user.Photos,
                FollowersCount = user.Followers.Count,
                FollowingCount = user.Followings.Count,
            };

            if (currentUser.Followings.Any(f => f.TargetId == user.Id))
            {
                profile.IsFollowed = true;
            }

            return profile;
        }
    }
}
