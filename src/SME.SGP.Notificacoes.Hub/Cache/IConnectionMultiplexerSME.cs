using StackExchange.Redis;

namespace SME.SGP.Notificacoes.Hub
{
    public interface IConnectionMultiplexerSME
    {
        IDatabase GetDatabase();
    }
}
