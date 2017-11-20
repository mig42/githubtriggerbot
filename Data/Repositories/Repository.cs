using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using githubtriggerbot.Data.Users;

namespace githubtriggerbot.Data.Repositories
{
    public class Repository
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public int  ProviderRepoId { get; set; }
    }
}
