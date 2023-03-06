using StackExchange.Redis;

namespace SME.SGP.Infra.Utilitarios
{
    public class RedisOptions
    {
        public const string Secao = "Redis";

        public string Endpoint { get; set; }
        public Proxy Proxy { get; set; }
        public int SyncTimeout { get; set; } = 5000;
        public string Prefixo { get; set; } = "SGP_";
    }
}
