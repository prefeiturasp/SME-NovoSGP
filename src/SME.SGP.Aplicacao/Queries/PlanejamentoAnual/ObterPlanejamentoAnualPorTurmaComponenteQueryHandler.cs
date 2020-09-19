using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorTurmaComponenteQueryHandler : IRequestHandler<ObterPlanejamentoAnualPorTurmaComponenteQuery, PlanejamentoAnualDto>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterPlanejamentoAnualPorTurmaComponenteQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }
        public async Task<PlanejamentoAnualDto> Handle(ObterPlanejamentoAnualPorTurmaComponenteQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnual.ObterPorTurmaEComponenteCurricular(request.TurmaId,request.ComponenteCurricularId);
        }
    }
}
