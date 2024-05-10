using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQueryHandler : IRequestHandler<ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery, IEnumerable<PlanejamentoAnualPeriodoEscolar>>
    {
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;

        public ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQueryHandler(IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar)
        {
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolar>> Handle(ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnualPeriodoEscolar.ObterCompletoPorIdAsync(request.Ids);
        }
    }
}
