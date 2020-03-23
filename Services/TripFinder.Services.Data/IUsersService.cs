namespace TripFinder.Services.Data
{
    public interface IUsersService
    {
        T GetById<T>(string id);
    }
}
