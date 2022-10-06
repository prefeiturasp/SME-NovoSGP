using StackExchange.Redis;

namespace SME.SGP.Infra.Interfaces
{
    public interface IConnectionMultiplexerSME
    {
        IDatabase GetDatabase();
    }
}
