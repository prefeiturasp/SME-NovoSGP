using Microsoft.Extensions.ObjectPool;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using RabbitMQ.Client;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public abstract class ConexoesRabbitFake : IConexoesRabbit
    {
        public ObjectPool<IModel> pool { get; set; }

        protected ConexoesRabbitFake(ConfiguracaoRabbit configuracaoRabbit, ObjectPoolProvider poolProvider)
        {

        }

        public IModel Get()
        {
            return null;
        }

        public void Return(RabbitMQ.Client.IModel conexao)
        {
            throw new System.NotImplementedException();
        }
    }
}