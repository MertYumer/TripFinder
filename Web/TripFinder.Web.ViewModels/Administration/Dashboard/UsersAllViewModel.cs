namespace TripFinder.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    public class UsersAllViewModel
    {
        public UsersAllViewModel()
        {
            this.Users = new HashSet<UserViewModel>();
        }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
