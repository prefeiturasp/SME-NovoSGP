using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculasValidasQueryHandler : IRequestHandler<ObterTurmasComMatriculasValidasQuery, IEnumerable<string>>
    {
        private readonly IMediator mediator;

        public ObterTurmasComMatriculasValidasQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<string>> Handle(ObterTurmasComMatriculasValidasQuery request, CancellationToken cancellationToken)
        {
            var turmasCodigosComMatriculasValidas = new List<string>();
            
            foreach (string codTurma in request.TurmasCodigos)
            {
                var matriculasAluno = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(codTurma, request.AlunoCodigo));
                if (matriculasAluno != null || matriculasAluno.Any())
                {
                    if ((matriculasAluno != null || matriculasAluno.Any()) && matriculasAluno.Any(m => m.PossuiSituacaoAtiva() || (!m.PossuiSituacaoAtiva() && m.DataSituacao >= request.PeriodoInicio && m.DataSituacao <= request.PeriodoFim) || (!m.PossuiSituacaoAtiva() && m.DataMatricula <= request.PeriodoFim && m.DataSituacao > request.PeriodoFim)))
                        turmasCodigosComMatriculasValidas.Add(codTurma);
                }
            }
            return turmasCodigosComMatriculasValidas;
        }
    }
}
