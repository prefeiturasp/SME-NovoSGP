using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorTipoCalendarioIdQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorTipoCalendarioIdQuery, IEnumerable<Dominio.PeriodoEscolar>>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodosEscolaresPorTipoCalendarioIdQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public Task<IEnumerable<Dominio.PeriodoEscolar>> Handle(ObterPeriodosEscolaresPorTipoCalendarioIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioPeriodoEscolar.ObterPorTipoCalendarioAsync(request.TipoCalendarioId);
        }
    }
}
