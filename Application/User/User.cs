using Application.Interfaces;
using Domain;
using System.Text.Json.Serialization;

namespace Application.User
{
    public class User
    {
        public User(AppUser appUser, IJwtGenerator jwtGenerator, string refreshToken)
        {
            DisplayName = appUser.DisplayName;
            Username = appUser.UserName;
            Image = appUser.MainPhotoUrl;
            RefreshToken = refreshToken;
            Token = jwtGenerator.CreateToken(appUser);
        }

        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
