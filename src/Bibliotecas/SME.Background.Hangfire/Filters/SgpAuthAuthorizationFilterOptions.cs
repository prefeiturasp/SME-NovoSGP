using Hangfire.Dashboard.BasicAuthorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Hangfire
{
    public class SgpAuthAuthorizationFilterOptions : BasicAuthAuthorizationFilterOptions
    {
        public static string AdminUser = "admin";
        public SgpAuthAuthorizationFilterOptions()
        {
            RequireSsl = false;
            SslRedirect = false;
            LoginCaseSensitive = true;
            Users = new[]
            {
                new BasicAuthAuthorizationUser
                {
                    Login = AdminUser,
                    PasswordClear =  "Sgp@amcom"
                },
                new BasicAuthAuthorizationUser
                {
                    Login = "user",
                    PasswordClear =  "Sgp@1234"
                },
            };
        }
    }
}
