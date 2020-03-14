using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TripFinder.Web.Areas.Identity.IdentityHostingStartup))]

namespace TripFinder.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
