using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorTipoCalendarioQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorTipoCalendarioQuery, IEnumerable<PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodosEscolaresPorTipoCalendarioQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<IEnumerable<PeriodoEscolar>> Handle(ObterPeriodosEscolaresPorTipoCalendarioQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterPorTipoCalendario(request.TipoCalendarioId);
    }
}
