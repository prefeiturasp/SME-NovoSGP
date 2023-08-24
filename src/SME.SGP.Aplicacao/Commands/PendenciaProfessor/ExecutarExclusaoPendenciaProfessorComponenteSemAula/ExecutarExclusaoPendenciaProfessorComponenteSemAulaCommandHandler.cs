using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommandHandler : IRequestHandler<ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommand request, CancellationToken cancellationToken)
        {
            var componentes = new long[] { request.ComponenteCurricularId };
            var periodoEscolarId = await mediator.Send(new ObterPeriodoEscolarIdPorTurmaIdQuery(request.Turma, request.DataAula));
            var pendenciasProfessores = await mediator.Send(new ObterPendenciasProfessorPorTurmaEComponenteQuery(request.Turma.Id,
                                                                                                                 componentes,
                                                                                                                 periodoEscolarId,
                                                                                                                 TipoPendencia.ComponenteSemAula));

            foreach (var pendenciaProfessor in pendenciasProfessores)
            {
                await mediator.Send(new ExcluirPendenciaProfessorCommand(pendenciaProfessor));
            }

            return true;
        }
    }
}
