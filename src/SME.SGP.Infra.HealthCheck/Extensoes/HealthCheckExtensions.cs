using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace SME.SGP.Infra
{
    public static class HealthCheckExtensions
    {
        public static HealthChecksBuilderSgp AddHealthChecksSgp(this IServiceCollection services)
        {
            return new HealthChecksBuilderSgp(services);
        }
        
        public static HealthChecksUiBuilderSgp AddHealthChecksUiSgp(this IServiceCollection services)
        {
            return new HealthChecksUiBuilderSgp(services);
        }

        public static void UseHealthChecksSgp(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }).UseHealthChecksUI();
        }        
    }
}