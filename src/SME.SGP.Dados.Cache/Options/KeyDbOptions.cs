using StackExchange.Redis;

namespace SME.SGP.Infra.Utilitarios
{
    public class KeyDbOptions
    {
        public const string Secao = "KeyDB";

        public string EndPoint_AB { get; set; }
        public string Prefixo_AB { get; set; }
        public int? SyncTimeout_AB { get; set; }

        public RedisOptions ObterOpcoesAbrangencia(RedisOptions redisPrincipal)
        {
            redisPrincipal ??= new RedisOptions();

            if (string.IsNullOrWhiteSpace(EndPoint_AB))
                return redisPrincipal;

            return new RedisOptions
            {
                Endpoint = EndPoint_AB,
                Proxy = Proxy.None,
                SyncTimeout = SyncTimeout_AB ?? redisPrincipal.SyncTimeout,
                Prefixo = string.IsNullOrWhiteSpace(Prefixo_AB) ? redisPrincipal.Prefixo : Prefixo_AB
            };
        }
    }
}
