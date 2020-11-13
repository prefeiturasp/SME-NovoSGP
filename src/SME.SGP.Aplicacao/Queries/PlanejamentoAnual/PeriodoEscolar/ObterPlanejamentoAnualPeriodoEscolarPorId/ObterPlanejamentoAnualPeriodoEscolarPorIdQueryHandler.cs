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
    public class ObterPlanejamentoAnualPeriodoEscolarPorIdQueryHandler : IRequestHandler<ObterPlanejamentoAnualPeriodoEscolarPorIdQuery, PlanejamentoAnualPeriodoEscolar>
    {
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;

        public ObterPlanejamentoAnualPeriodoEscolarPorIdQueryHandler(IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar)
        {
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
        }

        public async Task<PlanejamentoAnualPeriodoEscolar> Handle(ObterPlanejamentoAnualPeriodoEscolarPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnualPeriodoEscolar.ObterPorIdAsync(request.Id);
        }
    }
}
