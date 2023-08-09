using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.SGP.Infra.Utilitarios;
using System;

namespace SME.SGP.Infra
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(ConfiguracaoRabbit configuracaoRabbitOptions)
        {
            _connection = CreateConnection(configuracaoRabbitOptions ??
                                    throw new ArgumentNullException(nameof(configuracaoRabbitOptions)));
        }

        private IConnection CreateConnection(ConfiguracaoRabbit configuracaoRabbit)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Port = configuracaoRabbit.Port,
                HostName = configuracaoRabbit.HostName,
                UserName = configuracaoRabbit.UserName,
                Password = configuracaoRabbit.Password,
                VirtualHost = configuracaoRabbit.VirtualHost
            };
            return connectionFactory.CreateConnection();
        }

        public IModel Create()
        {
            var model = _connection.CreateModel();
            model.ConfirmSelect();
            return model;
        }

        public bool Return(IModel model)
        {
            if (model.IsOpen)
            {
                return true;
            }

            model.Dispose();
            return false;
        }
    }
}