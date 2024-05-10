using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasAusenciaAvaliacaoCommandHandler : IRequestHandler<VerificaExclusaoPendenciasAusenciaAvaliacaoCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificaExclusaoPendenciasAusenciaAvaliacaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaExclusaoPendenciasAusenciaAvaliacaoCommand request, CancellationToken cancellationToken)
        {
            var periodoEscolarId = await ObterPeriodoEscolar(request.TurmaCodigo, request.DataAvaliacao);
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));

            var pendenciasProfessores = await mediator.Send(new ObterPendenciasProfessorPorTurmaEComponenteQuery(turma.Id,
                                                                                                                 request.ComponentesCurriculares.Select(a => long.Parse(a)).ToArray(),
                                                                                                                 periodoEscolarId,
                                                                                                                 request.TipoPendencia));

            foreach (var pendenciaProfessor in pendenciasProfessores)
            {
                await mediator.Send(new ExcluirPendenciaProfessorCommand(pendenciaProfessor));
            }

            return true;
        }

        private async Task<long> ObterPeriodoEscolar(string turmaCodigo, DateTime dataReferencia)
            => await mediator.Send(new ObterPeriodoEscolarIdPorTurmaQuery(turmaCodigo, dataReferencia));
    }
}
