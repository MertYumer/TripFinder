namespace TripFinder.Services.Data.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    using AutoMapper;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TripFinder.Data;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Data.Repositories;
    using TripFinder.Services.Mapping;
    using TripFinder.Web;
    using TripFinder.Web.ViewModels.Cars;

    public abstract class BaseServiceTests : IDisposable
    {
        protected BaseServiceTests()
        {
            this.Configuration = this.SetConfiguration();
            var services = this.SetServices();
            this.RegisterMappings();
            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        protected ApplicationDbContext DbContext { get; set; }

        protected IConfigurationRoot Configuration { get; set; }

        public void Dispose()
        {
            this.DbContext.Database.EnsureDeleted();
            this.SetServices();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services
                 .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                 {
                     options.Password.RequireDigit = false;
                     options.Password.RequireLowercase = false;
                     options.Password.RequireUppercase = false;
                     options.Password.RequireNonAlphanumeric = false;
                     options.Password.RequiredLength = 6;
                 })
                 .AddEntityFrameworkStores<ApplicationDbContext>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Application services
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IConfiguration>(this.Configuration);

            Account account = new Account(
                this.Configuration["Cloudinary:AppName"],
                this.Configuration["Cloudinary:AppKey"],
                this.Configuration["Cloudinary:AppSecret"]);

            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITripsService, TripsService>();
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IReviewsService, ReviewsService>();

            var context = new DefaultHttpContext();
            services.AddSingleton<IHttpContextAccessor>(new HttpContextAccessor { HttpContext = context });

            return services;
        }

        private void RegisterMappings()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(CarDetailsViewModel).GetTypeInfo().Assembly);
        }

        private IConfigurationRoot SetConfiguration()
        {
            return new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath("../../../"))
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();
        }
    }
}
