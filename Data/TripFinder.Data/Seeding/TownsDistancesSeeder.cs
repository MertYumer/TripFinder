namespace TripFinder.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using TripFinder.Data.Models;

    public class TownsDistancesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.TownsDistances.Any())
            {
                return;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "towns-info.json");
            var townsDistances = new List<TownsDistance>();

            using (StreamReader reader = new StreamReader(path))
            {
                var jsonData = reader.ReadToEnd();
                townsDistances = JsonConvert.DeserializeObject<List<TownsDistance>>(jsonData);

                await dbContext.TownsDistances.AddRangeAsync(townsDistances);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
