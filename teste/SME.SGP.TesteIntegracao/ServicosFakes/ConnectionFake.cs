
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace SME.SGP.TesteIntegracao
{
    public class ConnectionFake : IConnection
    {
        public ushort ChannelMax => throw new NotImplementedException();

        public IDictionary<string, object> ClientProperties => throw new NotImplementedException();

        public ShutdownEventArgs CloseReason => throw new NotImplementedException();

        public AmqpTcpEndpoint Endpoint => throw new NotImplementedException();

        public uint FrameMax => throw new NotImplementedException();

        public TimeSpan Heartbeat => throw new NotImplementedException();

        public bool IsOpen => throw new NotImplementedException();

        public AmqpTcpEndpoint[] KnownHosts => throw new NotImplementedException();

        public IProtocol Protocol => throw new NotImplementedException();

        public IDictionary<string, object> ServerProperties => throw new NotImplementedException();

        public IList<ShutdownReportEntry> ShutdownReport => throw new NotImplementedException();

        public string ClientProvidedName => throw new NotImplementedException();

        public int LocalPort => throw new NotImplementedException();

        public int RemotePort => throw new NotImplementedException();

        public event EventHandler<CallbackExceptionEventArgs> CallbackException;
        public event EventHandler<ConnectionBlockedEventArgs> ConnectionBlocked;
        public event EventHandler<ShutdownEventArgs> ConnectionShutdown;
        public event EventHandler<EventArgs> ConnectionUnblocked;

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Abort(ushort reasonCode, string reasonText)
        {
            throw new NotImplementedException();
        }

        public void Abort(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void Abort(ushort reasonCode, string reasonText, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Close(ushort reasonCode, string reasonText)
        {
            throw new NotImplementedException();
        }

        public void Close(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void Close(ushort reasonCode, string reasonText, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public IModel CreateModel()
        {
            return new ModelFake();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void HandleConnectionBlocked(string reason)
        {
            throw new NotImplementedException();
        }

        public void HandleConnectionUnblocked()
        {
            throw new NotImplementedException();
        }

        public void UpdateSecret(string newSecret, string reason)
        {
            throw new NotImplementedException();
        }
    }
}
