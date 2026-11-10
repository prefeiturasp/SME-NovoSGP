using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SME.SGP.Dados.Cache;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;

namespace SME.SGP.IoC
{
    public static class RegistrarCache
    {
        public static void ConfigurarCache(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.EhNulo())
                return;

            services.AddOptions<CircuitBreakerSimplesOptions>()
                .Bind(configuration.GetSection(CircuitBreakerSimplesOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<CircuitBreakerSimplesOptions>();

            services.AddSingleton<ICircuitBreaker>(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<CircuitBreakerSimplesOptions>>()?.Value;
                return new CircuitBreaker(
                    limiteFalhas: options?.LimiteFalhas ?? 5,
                    tempoAbertura: TimeSpan.FromSeconds(options?.TempoAberturaSegundos ?? 30));
            });

            services.AddOptions<ConfiguracaoCacheOptions>()
                .Bind(configuration.GetSection(ConfiguracaoCacheOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<ConfiguracaoCacheOptions>();

            services.AddOptions<RedisOptions>()
                .Bind(configuration.GetSection(RedisOptions.Secao), c => c.BindNonPublicProperties = true);
            services.AddSingleton<RedisOptions>();

            services.AddOptions<KeyDbOptions>()
                .Bind(configuration.GetSection(KeyDbOptions.Secao), c => c.BindNonPublicProperties = true);

            services.RegistraMemoryCache();

            services.AddSingleton(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<ConfiguracaoCacheOptions>>()?.Value;
                var servicoTelemetria = serviceProvider.GetService<IServicoTelemetria>();
                var servicoMensageriaLogs = serviceProvider.GetService<IServicoMensageriaLogs>();
                var metricasCache = serviceProvider.GetService<IMetricasCache>();
                var circuitBreaker = serviceProvider.GetService<ICircuitBreaker>();

                return ObterRepositorio(serviceProvider, options, servicoTelemetria, services, servicoMensageriaLogs, metricasCache, circuitBreaker);
            });

            services.AddSingleton<IRepositorioCacheAbrangencia>(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<ConfiguracaoCacheOptions>>()?.Value;
                var servicoTelemetria = serviceProvider.GetService<IServicoTelemetria>();
                var servicoMensageriaLogs = serviceProvider.GetService<IServicoMensageriaLogs>();
                var metricasCache = serviceProvider.GetService<IMetricasCache>();

                return ObterRepositorioAbrangencia(serviceProvider, options, servicoTelemetria, servicoMensageriaLogs, metricasCache);
            });
        }

        private static void RegistraMemoryCache(this IServiceCollection services)
        {
            // Sempre registrado: além do cache em memória (UtilizaRedis = false), é usado para guardar
            // por 1 min a configuração do cache de abrangência (parâmetro UtilizarCacheAbrangencia).
            services.AddMemoryCache();
        }

        private static IRepositorioCacheAbrangencia ObterRepositorioAbrangencia(IServiceProvider serviceProvider, ConfiguracaoCacheOptions options, IServicoTelemetria servicoTelemetria, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache)
        {
            if (options.EhNulo() || !options.UtilizaRedis)
                return new RepositorioCacheAbrangenciaMemoria(serviceProvider.GetService<IMemoryCache>(), servicoTelemetria, servicoMensageriaLogs, metricasCache);

            var redisPrincipal = serviceProvider.GetService<IOptions<RedisOptions>>()?.Value;
            var keyDbOptions = serviceProvider.GetService<IOptions<KeyDbOptions>>()?.Value ?? new KeyDbOptions();
            var redisOptions = keyDbOptions.ObterOpcoesAbrangencia(redisPrincipal);
            var connection = new ConnectionMultiplexerSME(redisOptions);

            // Circuit breaker próprio: falha neste Redis não abre o breaker do Redis principal (e vice-versa).
            var circuitBreakerOptions = serviceProvider.GetService<IOptions<CircuitBreakerSimplesOptions>>()?.Value;
            var circuitBreaker = new CircuitBreaker(
                limiteFalhas: circuitBreakerOptions?.LimiteFalhas ?? 5,
                tempoAbertura: TimeSpan.FromSeconds(circuitBreakerOptions?.TempoAberturaSegundos ?? 30));

            return new RepositorioCacheAbrangenciaRedis(connection, servicoTelemetria, servicoMensageriaLogs, metricasCache, redisOptions, circuitBreaker);
        }

        private static IRepositorioCache ObterRepositorio(IServiceProvider serviceProvider, ConfiguracaoCacheOptions options, IServicoTelemetria servicoTelemetria, IServiceCollection services, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache, ICircuitBreaker circuitBreaker)
            => options.UtilizaRedis ?
                ObterRepositorioRedis(serviceProvider, servicoTelemetria,servicoMensageriaLogs, metricasCache, circuitBreaker) :
                ObterRepositorioMemory(serviceProvider, servicoTelemetria, servicoMensageriaLogs, metricasCache);

        private static IRepositorioCache ObterRepositorioRedis(IServiceProvider serviceProvider, IServicoTelemetria servicoTelemetria, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache, ICircuitBreaker circuitBreaker)
        {
            var redisOptions = serviceProvider.GetService<IOptions<RedisOptions>>()?.Value;
            var connection = new ConnectionMultiplexerSME(redisOptions);
            return new RepositorioCacheRedis(connection, servicoTelemetria, servicoMensageriaLogs, metricasCache, redisOptions, circuitBreaker);
        }

        private static IRepositorioCache ObterRepositorioMemory(IServiceProvider serviceProvider, IServicoTelemetria servicoTelemetria, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache)
        {
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            return new RepositorioCacheMemoria(memoryCache, servicoTelemetria, servicoMensageriaLogs, metricasCache);
        }
    }
}
