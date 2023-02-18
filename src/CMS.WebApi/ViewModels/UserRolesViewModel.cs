using System.Collections.Generic;

namespace CMS.WebApi.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string Username { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }

    public class UserRolesViewModel
    {
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}