using RabbitMQ.Client;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IConexoesRabbit
    {
        IModel Get();
        void Return(IModel conexao);
    }

    public interface IConexoesRabbitFilasSGP : IConexoesRabbit { }
    public interface IConexoesRabbitFilasLog : IConexoesRabbit { }
}
