using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SME.SGP.Infra
{
    public class HealthChecksUiBuilderSgp
    {
        private readonly HealthChecksUIBuilder healthChecksUiBuilder;
        
        public HealthChecksUiBuilderSgp(IServiceCollection services)
        {
            healthChecksUiBuilder = services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(5);
                opt.MaximumHistoryEntriesPerEndpoint(10);
                opt.SetApiMaxActiveRequests(1);

                opt.AddHealthCheckEndpoint("Health-API Indicadores", "/healthz");
            }).AddInMemoryStorage();
        }

        public HealthChecksUiBuilderSgp AddPostgreSqlStorageSgp(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SGP_Postgres");
            healthChecksUiBuilder.AddPostgreSqlStorage(connectionString);
            return this;
        }

        public HealthChecksUIBuilder Builder()
        {
            return healthChecksUiBuilder;
        }
    }
}