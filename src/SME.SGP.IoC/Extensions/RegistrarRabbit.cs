using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    public static class RegistrarRabbit
    {
        public static void AddRabbit(this IServiceCollection services, ConfiguracaoRabbitOptions configuracaoRabbitOptions)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbitOptions.HostName,
                UserName = configuracaoRabbitOptions.UserName,
                Password = configuracaoRabbitOptions.Password,
                VirtualHost = configuracaoRabbitOptions.VirtualHost
            };

            services.AddSingleton(factory);
        }

        public static void ConfigurarRabbitParaLogs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConfiguracaoRabbitLogOptions>()
                .Bind(configuration.GetSection("ConfiguracaoRabbitLog"), c => c.BindNonPublicProperties = true)
                .Configure(options =>
                {
                    services.AddSingleton(options);
                });
        }

        public static void ConfigurarRabbit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConfiguracaoRabbitOptions>()
                .Bind(configuration.GetSection(nameof(ConfiguracaoRabbitOptions)), c => c.BindNonPublicProperties = true)
                .Configure(options =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = options.HostName,
                        UserName = options.UserName,
                        Password = options.Password,
                        VirtualHost = options.VirtualHost
                    };

                    services.AddSingleton(factory);
                    services.AddSingleton(options);
                });
        }
    }
}