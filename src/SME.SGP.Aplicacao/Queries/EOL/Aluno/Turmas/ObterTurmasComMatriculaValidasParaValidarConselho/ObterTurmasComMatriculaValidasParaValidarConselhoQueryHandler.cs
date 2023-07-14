using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (turma != null)
                {
                    var matriculasAluno = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(turmaCodigo, request.AlunoCodigo));
                    if (matriculasAluno != null || matriculasAluno.Any())
                    {
                        if ((matriculasAluno != null || matriculasAluno.Any()) && matriculasAluno.Any(m => m.PossuiSituacaoAtiva() || (!m.PossuiSituacaoAtiva() && m.DataSituacao >= request.PeriodoInicio && m.DataSituacao <= request.PeriodoFim) || (!m.PossuiSituacaoAtiva() && m.DataMatricula <= request.PeriodoFim && m.DataSituacao > request.PeriodoFim)))
                        {
                            if (turma.TipoTurma != TipoTurma.EdFisica || turma.TipoTurma == TipoTurma.EdFisica && !matriculasAluno.Any(m => m.CodigoSituacaoMatricula == SituacaoMatriculaAluno.DispensadoEdFisica))
                                turmasCodigosComMatriculasValidas.Add(turma.CodigoTurma);
                        }
                    }
                }
            }
            return turmasCodigosComMatriculasValidas;
        }
    }
}
