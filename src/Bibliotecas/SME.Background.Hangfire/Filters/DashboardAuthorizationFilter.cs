using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace SME.Background.Hangfire
{
    public class DashboardAuthorizationFilter : BasicAuthAuthorizationFilter
    {
        private readonly BasicAuthAuthorizationFilterOptions options;

        public DashboardAuthorizationFilter(BasicAuthAuthorizationFilterOptions options) : base(options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        internal bool ReadOnly(DashboardContext dashboardContext)
        {
            var context = dashboardContext.GetHttpContext();
            string header = context.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(header))
            {
                AuthenticationHeaderValue authValues = AuthenticationHeaderValue.Parse(header);
                string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                var parts = parameter.Split(':');

                return !string.Equals(parts[0], SgpAuthAuthorizationFilterOptions.AdminUser);
            }

            return true;
        }
    }
}
