namespace TripFinder.Services.Data
{
    using TripFinder.Web.ViewModels.Users;

    public interface IUsersService
    {
        UserDetailsViewModel GetUserById(string userId);
    }
}
