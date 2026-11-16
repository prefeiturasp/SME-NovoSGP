using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNovosTiposUEPorAnoQueryHandler : IRequestHandler<ObterNovosTiposUEPorAnoQuery, string>
    {
        // Parâmetro de novos tipos de UE (tipo 55) muda raramente: guardado em memória do pod por 1 min,
        // só quando o cache de abrangência está ligado (parâmetro UtilizarCacheAbrangencia).
        private static readonly TimeSpan TEMPO_CACHE_PARAMETRO = TimeSpan.FromMinutes(1);

        private readonly IRepositorioParametrosSistemaConsulta repositorioParametrosSistema;
        private readonly IMediator mediator;
        private readonly IMemoryCache memoryCache;

        public ObterNovosTiposUEPorAnoQueryHandler(IRepositorioParametrosSistemaConsulta repositorioParametrosSistema,
                                                   IMediator mediator,
                                                   IMemoryCache memoryCache)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema;
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<string> Handle(ObterNovosTiposUEPorAnoQuery request, CancellationToken cancellationToken)
        {
            var configuracaoCache = await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance, cancellationToken);

            if (configuracaoCache.EhNulo())
                return await repositorioParametrosSistema.ObterNovosTiposUEPorAno(request.AnoLetivo);

            var chave = string.Format(NomeChaveCache.PARAMETRO_NOVOS_TIPOS_UE, request.AnoLetivo);

            // Guarda também o null (sem novos tipos para o ano), para não consultar o banco a cada request.
            if (memoryCache.TryGetValue(chave, out string novosTiposUE))
                return novosTiposUE;

            novosTiposUE = await repositorioParametrosSistema.ObterNovosTiposUEPorAno(request.AnoLetivo);
            memoryCache.Set(chave, novosTiposUE, TEMPO_CACHE_PARAMETRO);

            return novosTiposUE;
        }
    }
}
