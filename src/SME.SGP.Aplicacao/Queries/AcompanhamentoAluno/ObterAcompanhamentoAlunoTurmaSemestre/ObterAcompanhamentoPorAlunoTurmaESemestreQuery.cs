using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoPorAlunoTurmaESemestreQuery : IRequest<IEnumerable<AcompanhamentoAlunoDto>>
    {
        public ObterAcompanhamentoPorAlunoTurmaESemestreQuery(string alunoCodigo, string turmaCodigo, int semestre)
        {
            AlunoCodigo = alunoCodigo;
            TurmaCodigo = turmaCodigo;
            Semestre = semestre;
        }

        public string AlunoCodigo { get; }
        public string TurmaCodigo { get; }
        public int Semestre { get; }
    }
}
