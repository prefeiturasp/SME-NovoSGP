using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Infra
{
    public abstract class ConexoesRabbit : IConexoesRabbit
    {
        public ObjectPool<IModel> pool { get; set; }

        protected ConexoesRabbit(ConfiguracaoRabbit configuracaoRabbit, ObjectPoolProvider poolProvider)
        {
            var policy = new RabbitModelPooledObjectPolicy(configuracaoRabbit);

            pool = poolProvider.Create(policy);
        }

        public IModel Get()
            => pool.Get();

        public void Return(IModel conexao)
            => pool.Return(conexao);
    }

    public class ConexoesRabbitFilasSGP : ConexoesRabbit, IConexoesRabbitFilasSGP
    {
        public ConexoesRabbitFilasSGP(ConfiguracaoRabbitOptions configuracaoRabbit, ObjectPoolProvider poolProvider) : base(configuracaoRabbit, poolProvider)
        { }
    }

    public class ConexoesRabbitFilasLog : ConexoesRabbit, IConexoesRabbitFilasLog
    {
        public ConexoesRabbitFilasLog(ConfiguracaoRabbitLogOptions configuracaoRabbit, ObjectPoolProvider poolProvider) : base(configuracaoRabbit, poolProvider)
        { }
    }

}
