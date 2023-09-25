using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Utilitarios;
using System;
using SME.SGP.Infra.Interface;
using SME.SGP.Dados.Cache;
using SME.SGP.Dominio;

namespace SME.SGP.IoC
{
    public static class RegistrarCache
    {
        public static void ConfigurarCache(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.EhNulo())
                return;

            services.AddOptions<ConfiguracaoCacheOptions>()
                .Bind(configuration.GetSection(ConfiguracaoCacheOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<ConfiguracaoCacheOptions>();

            services.AddOptions<RedisOptions>()
                .Bind(configuration.GetSection(RedisOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<RedisOptions>();

            services.AddSingleton(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<ConfiguracaoCacheOptions>>()?.Value;
                var servicoTelemetria = serviceProvider.GetService<IServicoTelemetria>();
                var servicoMensageriaLogs = serviceProvider.GetService<IServicoMensageriaLogs>();
                var metricasCache = serviceProvider.GetService<IMetricasCache>();

                return ObterRepositorio(serviceProvider, options, servicoTelemetria, services, servicoMensageriaLogs, metricasCache);
            });
        }

        private static IRepositorioCache ObterRepositorio(IServiceProvider serviceProvider, ConfiguracaoCacheOptions options, IServicoTelemetria servicoTelemetria, IServiceCollection services, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache)
            => options.UtilizaRedis ?
                ObterRepositorioRedis(serviceProvider, servicoTelemetria,servicoMensageriaLogs, metricasCache) :
                ObterRepositorioMemory(serviceProvider, servicoTelemetria, services,servicoMensageriaLogs, metricasCache);

        private static IRepositorioCache ObterRepositorioRedis(IServiceProvider serviceProvider, IServicoTelemetria servicoTelemetria, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache)
        {
            var redisOptions = serviceProvider.GetService<IOptions<RedisOptions>>()?.Value;
            var connection = new ConnectionMultiplexerSME(redisOptions);
            return new RepositorioCacheRedis(connection, servicoTelemetria, redisOptions, servicoMensageriaLogs, metricasCache);
        }

        private static IRepositorioCache ObterRepositorioMemory(IServiceProvider serviceProvider, IServicoTelemetria servicoTelemetria, IServiceCollection services, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache)
        {
            services.AddMemoryCache();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            return new RepositorioCacheMemoria(memoryCache, servicoTelemetria, servicoMensageriaLogs, metricasCache);
        }
    }
}
