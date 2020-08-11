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
    public class ObterPeridosEscolaresPorTipoCalendarioIdQueryHandler : IRequestHandler<ObterPeridosEscolaresPorTipoCalendarioIdQuery, IEnumerable<PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeridosEscolaresPorTipoCalendarioIdQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<IEnumerable<PeriodoEscolar>> Handle(ObterPeridosEscolaresPorTipoCalendarioIdQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterPorTipoCalendario(request.TipoCalendarioId);
    }
}
