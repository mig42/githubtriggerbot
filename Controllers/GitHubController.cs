using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using githubtriggerbot.Models;
using System.Collections.Generic;
using githubtriggerbot.Models.GitHubViewModels;

namespace githubtriggerbot.Controllers
{
    [Authorize]
    public class GitHubController : Controller
    {
        public GitHubController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ListRepos()
        {
            var github = new GitHubClient(new ProductHeaderValue("triggerbot-test"));

            var user = await _userManager.GetUserAsync(User);
            var token = await _userManager.GetAuthenticationTokenAsync(user, "GitHub", "access_token");

            github.Credentials = new Credentials(token);

            IEnumerable<Repository> repos = await github.Repository.GetAllForCurrent();

            var model = new GitHubRepositoryListViewModel
            {
                UserName = user.UserName,
                Repos = repos.Select(repo =>
                    new GitHubRepositoryViewModel { Name = repo.Name, Url = repo.HtmlUrl })
            };
            return View(model);
        }

        [HttpPost]
        // TODO change do async during implementation
        public IActionResult LinkTrigger(GitHubRepositoryViewModel repoModel)
        {
            // TODO
            return NotFound();
        }

        readonly UserManager<ApplicationUser> _userManager;
    }
}
