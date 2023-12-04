using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorTipoCalendarioIdQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorTipoCalendarioIdQuery, IEnumerable<Dominio.PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioCache repositorioCache;

        public ObterPeriodosEscolaresPorTipoCalendarioIdQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioCache repositorioCache)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<Dominio.PeriodoEscolar>> Handle(ObterPeriodosEscolaresPorTipoCalendarioIdQuery request, CancellationToken cancellationToken)
        {
            var chaveCache = string.Format(NomeChaveCache.PERIODOS_ESCOLARES_CALENDARIO, request.TipoCalendarioId);
                        
            return await repositorioCache
                .ObterAsync(chaveCache, async
                    () => await repositorioPeriodoEscolar.ObterPorTipoCalendarioAsync(request.TipoCalendarioId));
        }
    }
}
