using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMotivoPorTurmaAlunoComponenteCurricularQuery : IRequest<IEnumerable<JustificativaAlunoDto>>
    {
        public ObterMotivoPorTurmaAlunoComponenteCurricularQuery(long turmaId, long componenteCurricularId, long alunoCodigo)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AlunoCodigo { get; set; }
    }
}
