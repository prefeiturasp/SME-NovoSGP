using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorTurmaComponenteQueryHandler : IRequestHandler<ObterPlanejamentoAnualPorTurmaComponenteQuery, long>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterPlanejamentoAnualPorTurmaComponenteQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }

        public async Task<long> Handle(ObterPlanejamentoAnualPorTurmaComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioPlanejamentoAnual.ObterIdPorTurmaEComponenteCurricular(request.TurmaId, request.ComponenteCurricularId);
    }
}
