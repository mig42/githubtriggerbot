using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace githubtriggerbot.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string DisplayName { get; set; }
    }
}
