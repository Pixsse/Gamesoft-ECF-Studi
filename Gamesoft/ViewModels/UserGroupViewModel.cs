using Microsoft.AspNetCore.Mvc.Rendering;
using static Gamesoft.Services.AccountMgt;

namespace Gamesoft.ViewModels
{
    public class UserGroupViewModel
    {
        public UserGroup SelectedUserGroup { get; set; }
        public required List<SelectListItem> UserGroupOptions { get; set; }
    }
}
