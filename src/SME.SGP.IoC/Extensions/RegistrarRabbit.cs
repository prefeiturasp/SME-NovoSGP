using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    internal static class RegistrarRabbit
    {
        internal static void ConfigurarRabbitParaLogs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConfiguracaoRabbitLogOptions>()
                .Bind(configuration.GetSection(ConfiguracaoRabbitLogOptions.Secao), c => c.BindNonPublicProperties = true);

            services.AddSingleton<ConfiguracaoRabbitLogOptions>();
            services.AddSingleton<IConexoesRabbitFilasLog>(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<ConfiguracaoRabbitLogOptions>>().Value;
                var provider = serviceProvider.GetService<IOptions<DefaultObjectPoolProvider>>().Value;
                return new ConexoesRabbitFilasLog(options, provider);
            });
        }

        internal static void ConfigurarRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                return;

            services.AddOptions<ConfiguracaoRabbitOptions>()
                .Bind(configuration.GetSection(ConfiguracaoRabbitOptions.Secao), c => c.BindNonPublicProperties = true);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ConfiguracaoRabbitOptions>>().Value;

            services.AddSingleton<IConnectionFactory>(serviceProvider =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = options.HostName,                        
                        UserName = options.UserName,
                        Password = options.Password,
                        VirtualHost = options.VirtualHost
                    };

                    return factory;
                });
            services.AddSingleton<ConfiguracaoRabbitOptions>();

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IConexoesRabbitFilasSGP>(serviceProvider =>
            {
                var provider = serviceProvider.GetService<IOptions<DefaultObjectPoolProvider>>().Value;
                return new ConexoesRabbitFilasSGP(options, provider);
            });
        }
    }
}