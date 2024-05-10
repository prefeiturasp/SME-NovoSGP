using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorPlanejamentoAnualIdQueryHandler : IRequestHandler<ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery, IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>>
    {
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;

        public ObterPeriodosEscolaresPorPlanejamentoAnualIdQueryHandler(IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar)
        {
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> Handle(ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnualPeriodoEscolar.ObterPlanejamentosAnuaisPeriodosTurmaPorPlanejamentoAnualId(request.PlanejamentoAnualId);
        }
    }
}
