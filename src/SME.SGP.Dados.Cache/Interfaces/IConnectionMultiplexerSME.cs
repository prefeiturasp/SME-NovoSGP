using System.Net;
using StackExchange.Redis;

namespace SME.SGP.Infra.Interfaces
{
    public interface IConnectionMultiplexerSME
    {
        IDatabase GetDatabase();
        EndPoint[] GetEndPoints(bool configuredOnly = false);
        IServer GetServer(EndPoint endpoint, object asyncState = null);
    }
}
