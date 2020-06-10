using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Api
{
    public static class RegistrarRabbit
    {
        public static void AddRabbit(this IServiceCollection services)
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Hostname"),
                UserName = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Username"),
                Password = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Password")
            };

            var conexaoRabbit = factory.CreateConnection();
            IModel _channel = conexaoRabbit.CreateModel();
            services.AddSingleton(conexaoRabbit);
            services.AddSingleton(_channel);

            _channel.ExchangeDeclare(RotasRabbit.ExchangeListenerWorkerRelatorios, ExchangeType.Topic);
            _channel.QueueDeclare(RotasRabbit.FilaListenerSgp, false, false, false, null);

            //_channel.QueueBind(RotasRabbit.FilaWorkerRelatorios, RotasRabbit.Exchange, RotasRabbit.FilaRelatoriosSolicitados);

        }
    }
}