using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace githubtriggerbot.Models.GitHubViewModels
{
    public class GitHubRepositoryListViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<GitHubRepositoryViewModel> Repos { get; set; }
    }

    public class GitHubRepositoryViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }

        [Display(Name="Watch pull requests")]
        public bool IsLinked { get; set; }
    }
}
