using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static Gamesoft.Services.AccountMgt;

namespace Gamesoft.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int AccountId
        {
            get
            {
                var accountId = _httpContextAccessor.HttpContext.Session.GetInt32("AccountId");
                return (int)((accountId != null) ? accountId : 0);
            }
            set => _httpContextAccessor.HttpContext.Session.SetInt32("AccountId", value);
        }

        public UserGroup GroupId
        {
            get
            {
                var groupId = _httpContextAccessor.HttpContext.Session.GetInt32("UserGroupId");
                return (UserGroup)((groupId != null) ? groupId : 0);
            }
            set => _httpContextAccessor.HttpContext.Session.SetInt32("UserGroupId", (int)value);
        }
        public string BackToPage
        {
            get
            {
                var backToPage = _httpContextAccessor.HttpContext.Session.GetString("BackToPage");
                return (string)((backToPage != null) ? backToPage : String.Empty);
            }
            set => _httpContextAccessor.HttpContext.Session.SetString("BackToPage", value);
        }
    }
}
