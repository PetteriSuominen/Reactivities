﻿using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
namespace Application.User
{
    public class ExternalLogin
    {
        public class Query : IRequest<User>
        {
            public string accessToken { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IFacebookAccessor _facebookAccessor;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<AppUser> userManager, IFacebookAccessor facebookAccessor,
                IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _facebookAccessor = facebookAccessor;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var userInfo = await _facebookAccessor.FacebookLogin(request.accessToken);

                if (userInfo == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest,
                        new { User = "Problem validating token" });
                }

                var user = await _userManager.FindByEmailAsync(userInfo.Email);

                var refreshToken = _jwtGenerator.GenerateRefreshToken();

                if (user != null)
                {
                    user.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(user);
                    return new User(user, _jwtGenerator, refreshToken.Token);
                }


                user = new AppUser
                {
                    DisplayName = userInfo.Name,
                    Id = userInfo.Id,
                    Email = userInfo.Email,
                    UserName = "fb_" + userInfo.Id,
                    EmailConfirmed = true
                };

                var photo = new Photo
                {
                    Id = "fb_" + userInfo.Id,
                    Url = userInfo.Picture.Data.Url,
                    IsMain = true
                };

                user.Photos.Add(photo);
                user.RefreshTokens.Add(refreshToken);

                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    throw new RestException(System.Net.HttpStatusCode.InternalServerError, new
                    {
                        User = "Problem creating user"
                    });
                }

                return new User(user, _jwtGenerator, refreshToken.Token);
            }
        }
    }
}
