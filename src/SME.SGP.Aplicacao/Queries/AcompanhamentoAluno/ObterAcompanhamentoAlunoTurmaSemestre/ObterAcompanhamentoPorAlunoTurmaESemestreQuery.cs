using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoPorAlunoTurmaESemestreQuery : IRequest<AcompanhamentoAlunoSemestre>
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
