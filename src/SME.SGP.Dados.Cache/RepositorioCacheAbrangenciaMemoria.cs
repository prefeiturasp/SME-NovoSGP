using Microsoft.Extensions.Caching.Memory;
using SME.SGP.Dados.Cache;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheAbrangenciaMemoria : RepositorioCacheMemoria, IRepositorioCacheAbrangencia
    {
        public RepositorioCacheAbrangenciaMemoria(IMemoryCache memoryCache, IServicoTelemetria servicoTelemetria, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache)
            : base(memoryCache, servicoTelemetria, servicoMensageriaLogs, metricasCache)
        {
            NomeServicoCache = "Cache Memória Abrangência";
        }
    }
}
