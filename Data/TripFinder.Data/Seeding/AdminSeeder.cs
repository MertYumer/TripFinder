namespace TripFinder.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using TripFinder.Common;
    using TripFinder.Data.Models;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            if (dbContext.ApplicationUsers.Any(u => u.UserName == "admin@tripfinder.com"))
            {
                return;
            }

            var user = new ApplicationUser
            {
                UserName = "admin@tripfinder.com",
                Email = "admin@tripfinder.com",
                FirstName = "AdminFirstName",
                LastName = "AdminLastName",
                PhoneNumber = "0881111111",
            };

            var password = "admin123";

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
