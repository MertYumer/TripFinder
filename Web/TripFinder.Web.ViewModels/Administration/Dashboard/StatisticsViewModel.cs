namespace TripFinder.Web.ViewModels.Administration.Dashboard
{
    public class StatisticsViewModel
    {
        public int AllUsersCount { get; set; }

        public int ActiveUsersCount { get; set; }

        public int DeletedUsersCount { get; set; }

        public int AllCarsCount { get; set; }

        public int CurrentCarsCount { get; set; }

        public int DeletedCarsCount { get; set; }

        public int AllTripsCount { get; set; }

        public int ActiveTripsCount { get; set; }

        public int DeletedTripsCount { get; set; }
    }
}
