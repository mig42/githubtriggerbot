using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace githubtriggerbot.Data.Users
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string DisplayName { get; set; }
    }
}
