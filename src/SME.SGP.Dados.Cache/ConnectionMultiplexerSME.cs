using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using StackExchange.Redis;
using System;

namespace SME.SGP.Infra.Contexto
{
    public class ConnectionMultiplexerSME : IConnectionMultiplexerSME
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public ConnectionMultiplexerSME(RedisOptions redisOptions)
        {
            try
            {
                var redisConfigurationOptions = new ConfigurationOptions()
                {
                    Proxy = redisOptions.Proxy,
                    SyncTimeout = redisOptions.SyncTimeout,
                    EndPoints = { redisOptions.Endpoint }
                };

                this.connectionMultiplexer = ConnectionMultiplexer
                    .Connect(redisConfigurationOptions);
            }
            catch (RedisConnectionException rcex)
            {
                //servicoLog.Registrar($"Erro de conexão com o servidor Redis. {rcex}");
            }
            catch (Exception ex)
            {
                //servicoLog.Registrar(ex);
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
