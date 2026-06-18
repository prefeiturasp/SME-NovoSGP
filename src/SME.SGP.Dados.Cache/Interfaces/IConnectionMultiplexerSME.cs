using StackExchange.Redis;
using System.Net;

namespace SME.SGP.Infra.Interfaces
{
    public interface IConnectionMultiplexerSME
    {
        IDatabase GetDatabase();
        EndPoint[] GetEndPoints(bool configuredOnly = false);
        IServer GetServer(EndPoint endpoint, object asyncState = null);
    }
}
