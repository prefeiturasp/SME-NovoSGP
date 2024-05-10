using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandler : IRequestHandler<ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery, PlanejamentoAnual>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }

        public async Task<PlanejamentoAnual> Handle(ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnual.ObterPlanejamentoAnualPorAnoEscolaBimestreETurma(request.TurmaId, request.PeriodoEscolarId, request.ComponenteCurricularId);
        }
    }
}
