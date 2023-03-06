using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ConnectionFactoryFake : ConnectionFactoryBase, IAsyncConnectionFactory
    {
        public bool DispatchConsumersAsync { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDictionary<string, object> ClientProperties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ushort RequestedChannelMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public uint RequestedFrameMax { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan RequestedHeartbeat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool UseBackgroundThreadsForIO { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UserName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string VirtualHost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Uri Uri { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ClientProvidedName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan HandshakeContinuationTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan ContinuationTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IAuthMechanismFactory AuthMechanismFactory(IList<string> mechanismNames)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection()
        {
            return new ConnectionFake();
        }

        public IConnection CreateConnection(string clientProvidedName)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection(IList<string> hostnames)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection(IList<string> hostnames, string clientProvidedName)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection(IList<AmqpTcpEndpoint> endpoints)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection(IList<AmqpTcpEndpoint> endpoints, string clientProvidedName)
        {
            throw new NotImplementedException();
        }
    }
}
