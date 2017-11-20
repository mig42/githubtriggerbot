using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using githubtriggerbot.Data.Repositories;
using githubtriggerbot.Data.Users;
using System;

namespace githubtriggerbot.Data
{
    public static class SqliteDbInitializer
    {
        // SQLite tables don't allow being set foreign keys after creation
        // We make sure the app starts from a clean slate
        public static void ClearDatabases(IServiceProvider serviceProvider)
        {
            ClearDatabase<UsersDbContext>(serviceProvider);
            ClearDatabase<RepositoriesDbContext>(serviceProvider);
        }

        static void ClearDatabase<T>(IServiceProvider serviceProvider) where T : DbContext
        {
            var context = serviceProvider.GetService<T>();
            context.Database.EnsureDeletedAsync();
            context.Database.EnsureCreatedAsync();
        }
    }
}
