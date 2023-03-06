using RabbitMQ.Client;

namespace SME.SGP.Infra.Interfaces
{
    public interface IConexoesRabbit
    {
        IModel Get();
        void Return(IModel conexao);
    }

    public interface IConexoesRabbitFilasSGP : IConexoesRabbit { }
    public interface IConexoesRabbitFilasLog : IConexoesRabbit { }
}
