using SME.SGP.Infra.Interfaces;
using StackExchange.Redis;
using System;

namespace SME.SGP.Infra.Contexto
{
    public class ConnectionMultiplexerSME : IConnectionMultiplexerSME
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public ConnectionMultiplexerSME(string host, IServicoLog servicoLog)
        {
            try
            {
                this.connectionMultiplexer = ConnectionMultiplexer
                    .Connect(string.Concat(host, $",ConnectTimeout={TimeSpan.FromSeconds(1).TotalMilliseconds}"));
            }
            catch (RedisConnectionException rcex)
            {
                servicoLog.Registrar($"Erro de conexão com o servidor Redis. {rcex}");
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
