using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using StackExchange.Redis;
using System;
using System.Net;

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
                //Ignorar exceção
            }
            catch (Exception ex)
            {
                //Ignorar exceção
            }
        }

        public IDatabase GetDatabase()
        {
            if (connectionMultiplexer is not { IsConnected: true } || connectionMultiplexer.IsConnecting)
                return null;

            return connectionMultiplexer.GetDatabase();
        }

        public EndPoint[] GetEndPoints(bool configuredOnly = false)
        {
            if (connectionMultiplexer is not { IsConnected: true } || connectionMultiplexer.IsConnecting)
                return null;

            return connectionMultiplexer.GetEndPoints(configuredOnly);
        }

        public IServer GetServer(EndPoint endpoint, object asyncState = null)
        {
            if (connectionMultiplexer is not { IsConnected: true } || connectionMultiplexer.IsConnecting)
                return null;

            return connectionMultiplexer.GetServer(endpoint, asyncState);
        }
    }
}
