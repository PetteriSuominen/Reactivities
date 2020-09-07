using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public virtual ICollection<UserActivity> UserActivities { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

        public string MainPhoto
        {
            get
            {
                return Photos?.FirstOrDefault(p => p.IsMain)?.Url;
            }
        }
    }
}
