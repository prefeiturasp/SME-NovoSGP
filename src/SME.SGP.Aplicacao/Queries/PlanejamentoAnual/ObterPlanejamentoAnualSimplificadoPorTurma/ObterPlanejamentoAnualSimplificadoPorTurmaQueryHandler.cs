using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualSimplificadoPorTurmaQueryHandler : IRequestHandler<ObterPlanejamentoAnualSimplificadoPorTurmaQuery, PlanejamentoAnualDto>
    {
        public readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterPlanejamentoAnualSimplificadoPorTurmaQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }
        public async Task<PlanejamentoAnualDto> Handle(ObterPlanejamentoAnualSimplificadoPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanejamentoAnual.ObterPlanejamentoAnualSimplificadoPorTurma(request.TurmaId);

        }
    }
}
