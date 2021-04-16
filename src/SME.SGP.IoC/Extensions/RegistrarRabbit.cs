using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;

namespace SME.SGP.IoC
{
    public static class RegistrarRabbit
    {
        public static void AddRabbit(this IServiceCollection services)
        {
            var configuracaoRabbit = new ConfiguracaoRabbit();

            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbit.HostName,  //Environment.GetEnvironmentVariable("ConfiguracaoRabbit__HostName"),
                UserName = configuracaoRabbit.UserName, //Environment.GetEnvironmentVariable("ConfiguracaoRabbit__UserName"),
                Password = configuracaoRabbit.Password, //Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Password"),
                VirtualHost = configuracaoRabbit.VirtualHost //Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Virtualhost")
            };

            var conexaoRabbit = factory.CreateConnection();
            IModel _channel = conexaoRabbit.CreateModel();
            //services.AddSingleton(conexaoRabbit);

            //services.AddTransient<IModel>(_channel);

            //_channel.ExchangeDeclare(RotasRabbit.ExchangeSgp, ExchangeType.Topic);
            //_channel.QueueDeclare(RotasRabbit.FilaSgp, false, false, false, null);
            //_channel.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, "*");
        }
    }
}