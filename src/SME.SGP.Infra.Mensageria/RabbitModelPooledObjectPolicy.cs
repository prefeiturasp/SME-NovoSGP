using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.SGP.Infra.Utilitarios;
using System;

namespace SME.SGP.Infra
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly IConnection conexao;

        public RabbitModelPooledObjectPolicy(ConfiguracaoRabbit configuracaoRabbitOptions)
        {
            conexao = GetConnection(configuracaoRabbitOptions ?? throw new ArgumentNullException(nameof(configuracaoRabbitOptions)));
        }

        private IConnection GetConnection(ConfiguracaoRabbit configuracaoRabbit)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "bitnami",
                VirtualHost = "dev"
            };
            return factory.CreateConnection();
        }

        public IModel Create()
        {
            var channel = conexao.CreateModel();
            channel.ConfirmSelect();
            return channel;
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
                return true;
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
