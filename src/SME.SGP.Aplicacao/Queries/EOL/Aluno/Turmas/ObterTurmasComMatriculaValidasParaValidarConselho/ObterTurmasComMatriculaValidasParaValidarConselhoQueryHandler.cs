using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculaValidasParaValidarConselhoQueryHandler : IRequestHandler<ObterTurmasComMatriculaValidasParaValidarConselhoQuery, IEnumerable<string>>
    {
        private readonly IMediator mediator;

        public ObterTurmasComMatriculaValidasParaValidarConselhoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<string>> Handle(ObterTurmasComMatriculaValidasParaValidarConselhoQuery request, CancellationToken cancellationToken)
        {
            var turmasCodigosComMatriculasValidas = new List<string>();

            foreach(var turmaCodigo in request.TurmasCodigos)
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
                
                if (turma.NaoEhNulo())
                {
                    var matriculasAluno = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(turmaCodigo, request.AlunoCodigo));
                    
                    if (PodeAdicionarTurma(matriculasAluno, turma, request))
                        turmasCodigosComMatriculasValidas.Add(turma.CodigoTurma);
                }
            }
            return turmasCodigosComMatriculasValidas;
        }

        private bool PodeAdicionarTurma(IEnumerable<AlunoPorTurmaResposta> matriculasAluno,
                                        Turma turma,
                                        ObterTurmasComMatriculaValidasParaValidarConselhoQuery request)
        {
            var existeMatricula = matriculasAluno.Any(m => m.PossuiSituacaoAtiva() || (m.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido &&
                                                           (!m.PossuiSituacaoAtiva() && m.DataSituacao >= request.PeriodoInicio && m.DataSituacao <= request.PeriodoFim) ||
                                                           (!m.PossuiSituacaoAtiva() && m.DataMatricula <= request.PeriodoFim && m.DataSituacao > request.PeriodoFim)));
            if (existeMatricula)
                return turma.TipoTurma != TipoTurma.EdFisica ||
                       turma.TipoTurma == TipoTurma.EdFisica &&
                       !matriculasAluno.Any(m => m.CodigoSituacaoMatricula == SituacaoMatriculaAluno.DispensadoEdFisica);

            return false;
        }
    }
}
