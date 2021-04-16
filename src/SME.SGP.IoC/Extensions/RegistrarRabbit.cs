using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    public static class RegistrarRabbit
    {
        public static void AddRabbit(this IServiceCollection services)
        {
            var configuracaoRabbit = new ConfiguracaoRabbitOptions();

            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbit.HostName,
                UserName = configuracaoRabbit.UserName,
                Password = configuracaoRabbit.Password,
                VirtualHost = configuracaoRabbit.VirtualHost
            };

            var conexaoRabbit = factory.CreateConnection();
            IModel _channel = conexaoRabbit.CreateModel();
        }
    }
}