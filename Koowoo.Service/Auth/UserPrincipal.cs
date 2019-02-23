using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Koowoo.Services.Auth
{
    public class UserPrincipal: ClientUserData,IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public string[] Roles { get; set; }

        public UserPrincipal(ClientUserData clientUserData)
        {
            this.Identity = new GenericIdentity(string.Format("{0}", clientUserData.UserId));

            this.UserId = clientUserData.UserId;
            this.UserName = clientUserData.UserName;
            this.UserPermission = clientUserData.UserPermission;
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public bool HasPermission(string permission)
        {
            if (UserId == 1)
                return true;
            return UserPermission.Contains(permission);
        }
    }
}
