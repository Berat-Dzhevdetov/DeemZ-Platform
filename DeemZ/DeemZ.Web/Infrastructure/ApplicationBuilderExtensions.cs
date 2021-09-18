namespace DeemZ.Web.Infrastructure
{
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using static DeemZ.Global.WebConstants.Constant;

    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedRoles(services);
            SeedResourceTypes(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<DeemZDbContext>();

            data.Database.Migrate();
        }

        private static void SeedResourceTypes(IServiceProvider services)
        {
            var data = services.GetRequiredService<DeemZDbContext>();

            if (data.ResourceTypes.Any()) return;
            data.ResourceTypes.AddRange(new[]
            {
                new ResourceType() { Name = "Youtube link", Icon = "<i class=\"fab fa-youtube\"></i>", IsRemote = true },
                new ResourceType() { Name = "Facebook link", Icon = "<i class=\"fab fa-facebook\"></i>", IsRemote = true },
                new ResourceType() { Name = "Word file", Icon = "<i class=\"fas fa-file-word\"></i>" },
                new ResourceType() { Name = "Presentation", Icon = "<i class=\"fas fa-file-powerpoint\"></i>" },
                new ResourceType() { Name = "Video", Icon = "<i class=\"fas fa-video\"></i>" }
            });

            data.SaveChanges();
        }

        private static void SeedRoles(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(Role.AdminRoleName))
                    return;

                var role = new IdentityRole { Name = Role.AdminRoleName };

                await roleManager.CreateAsync(role);

                var adminPass = "admin123";

                var user = new ApplicationUser()
                {
                    Email = "admin@deemz.com",
                    UserName = "admin",
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(user, adminPass);

                await userManager.AddToRoleAsync(user, role.Name);
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}