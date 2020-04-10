namespace TripFinder.Services.Data
{
    using System.Threading.Tasks;

    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Users;

    public interface IUsersService
    {
        T GetById<T>(string id);

        ApplicationUser GetById(string id);

        string CheckForUserById(string id);

        Task<string> UpdateAsync(UserEditInputModel inputModel);

        Task<string> DeleteAsync(string id);

        Task<string> SendNotificationAsync(string receiverId, string senderId, Trip trip, NotificationSubject subject);
    }
}
