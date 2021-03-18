using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoPorAlunoTurmaESemestreQuery : IRequest<IEnumerable<AcompanhamentoAlunoTurmaSemestreDto>>
    {
        public ObterAcompanhamentoPorAlunoTurmaESemestreQuery(string alunoCodigo, long turmaId, int semestre)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
            Semestre = semestre;
        }

        public string AlunoCodigo { get; }
        public long TurmaId { get; }
        public int Semestre { get; }
    }
}
