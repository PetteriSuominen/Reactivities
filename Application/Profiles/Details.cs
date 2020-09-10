using Application.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class Details
    {
        public class Query : IRequest<Profile>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Profile>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Profile> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

                if (user == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound,
                        new { Profile = "Not found" });
                }

                return new Profile
                {
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Bio = user.Bio,
                    Image = user.MainPhoto,
                    Photos = user.Photos
                };
            }
        }
    }
}
