using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace Koowoo.Services.Auth
{
    public class CurrentUserUtils
    {
        public static UserPrincipal CurrentPrincipal
        {
            get
            {
                return Thread.CurrentPrincipal as UserPrincipal;
               // return HttpContext.Current.User as UserPrincipal;
            }
        }
    }
}
