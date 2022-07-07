using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    internal static class RegistrarRabbit
    {
        internal static void ConfigurarRabbitParaLogs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConfiguracaoRabbitLogOptions>()
                .Bind(configuration.GetSection("ConfiguracaoRabbitLog"), c => c.BindNonPublicProperties = true);

            services.AddSingleton<ConfiguracaoRabbitLogOptions>();
        }

        internal static void ConfigurarRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                return;

            services.AddOptions<ConfiguracaoRabbitOptions>()
                .Bind(configuration.GetSection(nameof(ConfiguracaoRabbitOptions)), c => c.BindNonPublicProperties = true)
                .Services.AddSingleton<IConnectionFactory>(serviceProvider =>
                {
                    var options = serviceProvider.GetService<IOptions<ConfiguracaoRabbitOptions>>().Value;

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
            services.AddSingleton<ConfiguracaoRabbitLogOptions>();
        }
    }
}