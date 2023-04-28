using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class
        ObterTurmasComMatriculasValidasQueryHandler : IRequestHandler<ObterTurmasComMatriculasValidasQuery,
            IEnumerable<string>>
    {
        private readonly IMediator mediator;

        public ObterTurmasComMatriculasValidasQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<string>> Handle(ObterTurmasComMatriculasValidasQuery request,
            CancellationToken cancellationToken)
        {
            var turmasCodigosComMatriculasValidas = new List<string>();

            foreach (var codTurma in request.TurmasCodigos)
            {
                var matriculasAluno =
                    await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(codTurma, request.AlunoCodigo),
                        cancellationToken);

                if (matriculasAluno == null && !matriculasAluno.Any())
                    continue;

                if ((matriculasAluno != null || matriculasAluno.Any()) &&
                    matriculasAluno.Any(m => m.PossuiSituacaoAtiva() ||
                                             (!m.PossuiSituacaoAtiva() && m.DataSituacao >= request.PeriodoInicio)))
                {
                    turmasCodigosComMatriculasValidas.Add(codTurma);
                }
            }

            return turmasCodigosComMatriculasValidas;
        }
    }
}