using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Hangfire
{
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            // Nesse primeiro momento o permissionamento está liberado porém em produção o dashboard está como readonly
            // o ideal seria rever esse conceito 
            return true;
        }
    }
}
