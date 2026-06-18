using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dados.Cache;

namespace SME.SGP.IoC
{
    public static class RegistrarMetricas
    {
        public static void ConfigurarMetricasCache(this IServiceCollection services)
        {
            services.AddSingleton<IMetricasCache, MetricasCache>();
        }
    }
}
