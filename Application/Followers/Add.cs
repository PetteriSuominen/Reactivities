﻿using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Followers
{
    public class Add
    {
        public class Command : IRequest
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await _context.Users.SingleOrDefaultAsync(u =>
                    u.UserName == _userAccessor.GetCurrentUsername());

                var target = await _context.Users.SingleOrDefaultAsync(u =>
                    u.UserName == request.Username);

                if (target == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound,
                        new { User = "Not found" });
                }

                var following = await _context.Followings.SingleOrDefaultAsync(uf =>
                    uf.ObserverId == observer.Id && uf.TargetId == target.Id);

                if (following != null)
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest,
                        new { User = "You are already following this user" });
                }

                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target
                };

                _context.Followings.Add(following);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                    return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
