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

            var conexaoRabbit = factory.CreateConnection();
            IModel _channel = conexaoRabbit.CreateModel();            
            services.AddSingleton(conexaoRabbit);
            services.AddSingleton(_channel);
        }
    }
}