using RabbitMQ.Client;

namespace SME.SGP.Infra.Utilitarios
{
    public abstract class ConfiguracaoRabbit
    {
        private int _port;

        public static string Secao => "";
        public int Port
        {
            get
            {
                if (_port == 0)
                    _port = AmqpTcpEndpoint.UseDefaultPort; 
                return _port;
            }
            set
            {
                _port = value;
            }
        }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public ushort TempoHeartBeat { get; set; }
    }
}
