using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var matriculasAluno = await ObterMatriculasAlunoTurma(codTurma, request.AlunoCodigo, request.PeriodoInicio, cancellationToken);
                if (matriculasAluno.NaoEhNulo() || matriculasAluno.Any())
                {
                    if (matriculasAluno.Any(m => m.CodigoTurma.ToString() == codTurma &&
                       ((m.PossuiSituacaoAtiva() && m.DataMatricula <= request.PeriodoFim) 
                       || (!m.PossuiSituacaoAtiva() && m.DataSituacao >= request.PeriodoInicio && m.DataSituacao <= request.PeriodoFim) 
                       || (!m.PossuiSituacaoAtiva() && m.DataMatricula <= request.PeriodoFim && m.DataSituacao > request.PeriodoFim))))
                            turmasCodigosComMatriculasValidas.Add(codTurma);
                }
            }

            return turmasCodigosComMatriculasValidas;
        }

        private async Task<IEnumerable<AlunoPorTurmaResposta>> ObterMatriculasAlunoTurma(string codigoTurma, string codigoAluno, DateTime dataBaseInicio, CancellationToken cancellationToken)
        {
            if (dataBaseInicio.EhAnoAtual())
                return (await mediator
                        .Send(new ObterMatriculasAlunoNaTurmaQuery(codigoTurma, codigoAluno), cancellationToken))
                        .Where(m => m.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido);
            else
                return (await mediator
                        .Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(codigoTurma), int.Parse(codigoAluno)), cancellationToken))
                        .Where(m => m.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido);
        }
    }
}
