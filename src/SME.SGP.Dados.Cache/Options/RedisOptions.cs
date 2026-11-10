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

        // Redis dedicado ao cache de abrangência. Quando não informados, usam os valores do Redis principal.
        public string Endpoint2 { get; set; }
        public Proxy? Proxy2 { get; set; }
        public int? SyncTimeout2 { get; set; }
        public string Prefixo2 { get; set; }

        public RedisOptions ObterOpcoesAbrangencia()
            => new RedisOptions
            {
                Endpoint = string.IsNullOrWhiteSpace(Endpoint2) ? Endpoint : Endpoint2,
                Proxy = Proxy2 ?? Proxy,
                SyncTimeout = SyncTimeout2 ?? SyncTimeout,
                Prefixo = string.IsNullOrWhiteSpace(Prefixo2) ? Prefixo : Prefixo2
            };
    }
}
