using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQueryHandler : AbstractUseCase, IRequestHandler<ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery, IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual, IMediator mediator):base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Handle(ObterTurmasParaCopiaPlanejamentoAnualUsuarioCPQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.TurmaId.ToString()));
            var turmas = await repositorioPlanejamentoAnual.ObterTurmasParaCopiaPlanejamentoAnualCP(turma, turma.Ano, request.EnsinoEspecial, request.ConsideraHistorico);

            return turmas;
        }
    }
}
