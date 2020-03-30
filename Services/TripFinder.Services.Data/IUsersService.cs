namespace TripFinder.Services.Data
{
    using System.Threading.Tasks;

    using TripFinder.Web.ViewModels.Users;

    public interface IUsersService
    {
        T GetById<T>(string id);

        Task<string> UpdateAsync(UserEditInputModel inputModel);
    }
}
