using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasAusenciaFechamentoCommandHandler : IRequestHandler<VerificaExclusaoPendenciasAusenciaFechamentoCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificaExclusaoPendenciasAusenciaFechamentoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaExclusaoPendenciasAusenciaFechamentoCommand request, CancellationToken cancellationToken)
        {


            var pendenciasProfessores = await mediator.Send(new ObterPendenciasProfessorPorTurmaEComponenteQuery(request.TurmaId,
                                                                                                                 new long[] { request.DisciplinaId },
                                                                                                                 request.PeriodoEscolarId,
                                                                                                                 Dominio.TipoPendencia.AusenciaFechamento));

            foreach (var pendenciaProfessor in pendenciasProfessores)
            {
                await mediator.Send(new ExcluirPendenciaProfessorCommand(pendenciaProfessor));
            }

            return true;
        }
    }
}
