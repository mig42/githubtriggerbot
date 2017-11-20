using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace githubtriggerbot.Data.Repositories
{
    public class Hook
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Repository Repository { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string HookUri { get; set; }
    }
}
