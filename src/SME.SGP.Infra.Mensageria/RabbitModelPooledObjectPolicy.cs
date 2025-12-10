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
            conexao = CreateConnection(configuracaoRabbitOptions ?? throw new ArgumentNullException(nameof(configuracaoRabbitOptions)));
        }

        private IConnection CreateConnection(ConfiguracaoRabbit configuracaoRabbit)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Port = -1,
                HostName = "10.50.1.209",
                UserName = "usr_amcom",
                Password = "AMcom20anos",
                VirtualHost = "hom"
            };

            return connectionFactory.CreateConnection();
        }

        public IModel Create()
        {
            var model = conexao.CreateModel();
            model.ConfirmSelect();
            return model;
        }

        public bool Return(IModel model)
        {
            if (model.IsOpen)
                return true;

            model.Dispose();
            return false;
        }
    }
}
