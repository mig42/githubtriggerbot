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

            var repoList = await BuildRepoList(github, repos);

            return View(new GitHubRepositoryListViewModel
            {
                UserName = user.UserName,
                Repos = repoList
            });
        }

        [HttpPost]
        // TODO change do async during implementation
        public IActionResult LinkTrigger(GitHubRepositoryViewModel repoModel)
        {
            // TODO
            return NotFound();
        }

        async Task<IEnumerable<GitHubRepositoryViewModel>> BuildRepoList(
            GitHubClient client, IEnumerable<Repository> repos)
        {
            var result = new List<GitHubRepositoryViewModel>();
            foreach (var repo in repos)
            {
                result.Add(new GitHubRepositoryViewModel
                {
                    Name = repo.FullName,
                    Url = repo.HtmlUrl,
                    IsLinked = await IsPullRequestHookSet(client, repo)
                });
            }
            return result;
        }

        async Task<bool> IsPullRequestHookSet(GitHubClient client, Repository repository)
        {
            try
            {
                var hooks = await client.Repository.Hooks.GetAll(repository.Id);
                return hooks.Any(hook => GetHookFromDb(hook) != null);
            }
            catch (System.Exception)
            {
                // TODO log exception, most likely no hooks are set
                return false;
            }
        }

        // TODO
        // This will probably turn into async
        object GetHookFromDb(RepositoryHook hook)
        {
            return null;
        }

        readonly UserManager<ApplicationUser> _userManager;
    }
}
