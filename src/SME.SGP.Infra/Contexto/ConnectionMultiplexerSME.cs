using SME.SGP.Infra.Interfaces;
using StackExchange.Redis;
using System;
using System.Diagnostics;

namespace SME.SGP.Infra.Contexto
{
    public class ConnectionMultiplexerSME : IConnectionMultiplexerSME
    {
        private readonly string host;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public ConnectionMultiplexerSME(string host, IServicoLog servicoLog)
        {
            try
            {
                this.host = host;
                this.connectionMultiplexer = ConnectionMultiplexer.Connect(host);
            }
            catch (RedisConnectionException rcex)
            {
                servicoLog.Registrar(rcex);
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
            }
        }

        public IDatabase GetDatabase()
        {
            if (connectionMultiplexer == null || !connectionMultiplexer.IsConnected || connectionMultiplexer.IsConnecting)
                return null;

            return connectionMultiplexer.GetDatabase();
        }
    }
}
