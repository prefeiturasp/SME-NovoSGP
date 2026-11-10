using Microsoft.Extensions.Options;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Infra
{
    /// <summary>
    /// Verifica o Redis do cache de abrangência (Redis:Endpoint2; sem Endpoint2, o Redis principal).
    /// Mesma verificação do RedisCheck, sobre a conexão do IRepositorioCacheAbrangencia.
    /// </summary>
    public class RedisAbrangenciaCheck : RedisCheck
    {
        public RedisAbrangenciaCheck(IRepositorioCacheAbrangencia repositorioCacheAbrangencia, IOptions<RedisOptions> redisOptions,
            IOptions<ConfiguracaoCacheOptions> configuracaoCacheOptions)
            : base(repositorioCacheAbrangencia, redisOptions.Value?.ObterOpcoesAbrangencia(), configuracaoCacheOptions.Value)
        {
        }
    }
}
