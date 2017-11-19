using Microsoft.AspNetCore.Identity;

namespace githubtriggerbot.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string GitHubToken { get; set; }
        public string DisplayName { get; set; }
    }
}
