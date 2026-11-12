using Microsoft.Extensions.Options;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Infra
{
    public class RedisAbrangenciaCheck : RedisCheck
    {
        public RedisAbrangenciaCheck(IRepositorioCacheAbrangencia repositorioCacheAbrangencia, IOptions<RedisOptions> redisOptions,
            IOptions<KeyDbOptions> keyDbOptions, IOptions<ConfiguracaoCacheOptions> configuracaoCacheOptions)
            : base(repositorioCacheAbrangencia,
                  (keyDbOptions.Value ?? new KeyDbOptions()).ObterOpcoesAbrangencia(redisOptions.Value),
                  configuracaoCacheOptions.Value)
        {
        }
    }
}
