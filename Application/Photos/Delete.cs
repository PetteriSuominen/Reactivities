﻿using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Photos
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u
                    => u.UserName == _userAccessor.GetCurrentUsername());

                var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);

                if (photo == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound,
                        new { Photo = "Not found" });
                }

                if (photo.IsMain)
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest,
                        new { Photo = "You cannot delete your main photo" });
                }

                var result = _photoAccessor.DeletePhoto(request.Id);

                if (result == null)
                {
                    throw new Exception("Problem deleting the photo");
                }

                user.Photos.Remove(photo);

                var sucess = await _context.SaveChangesAsync() > 0;

                if (sucess) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
