using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaPlanejamentoAnualQueryHandler : AbstractUseCase, IRequestHandler<ObterTurmasParaCopiaPlanejamentoAnualQuery, IEnumerable<TurmaParaCopiaPlanoAnualDto>>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ObterTurmasParaCopiaPlanejamentoAnualQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual, IMediator mediator):base(mediator)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Handle(ObterTurmasParaCopiaPlanejamentoAnualQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(request.TurmaId));
            var planejamentoId = await repositorioPlanejamentoAnual.ObterTurmasParaCopiaPlanejamentoAnual(turma, turma.Ano, request.ComponenteCurricularId, request.RF, request.EnsinoEspecial, request.EhProfessor);

            return planejamentoId;
        }
    }
}
