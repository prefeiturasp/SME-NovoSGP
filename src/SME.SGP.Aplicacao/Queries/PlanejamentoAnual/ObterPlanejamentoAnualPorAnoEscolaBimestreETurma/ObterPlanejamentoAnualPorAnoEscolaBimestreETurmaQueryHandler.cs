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
    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandler : IRequestHandler<ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery, PlanejamentoAnual>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }

        public async Task<PlanejamentoAnual> Handle(ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnual.ObterPlanejamentoAnualPorAnoEscolaBimestreETurma(request.Ano, request.EscolaId, request.TurmaId, request.Bimestre, request.DisciplinaId);
        }
    }
}
