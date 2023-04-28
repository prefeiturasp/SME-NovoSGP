using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class RabbitModelPooledObjectPolicyFake : IPooledObjectPolicy<IModel>
    {
        private readonly IConnection conexao;

        public RabbitModelPooledObjectPolicyFake(ConfiguracaoRabbit configuracaoRabbitOptions)
        {
        }

        private IConnection GetConnection(ConfiguracaoRabbit configuracaoRabbit)
        {
            return new ConnectionFake();
        }

        public IModel Create()
        {
            return null;
        }

        public bool Return(IModel obj)
        {
            return true;
        }
    }
}