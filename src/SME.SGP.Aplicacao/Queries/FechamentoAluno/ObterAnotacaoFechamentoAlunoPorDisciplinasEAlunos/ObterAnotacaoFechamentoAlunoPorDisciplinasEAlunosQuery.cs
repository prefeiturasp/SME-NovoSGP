using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQuery : IRequest<IEnumerable<AnotacaoFechamentoAluno>>
    {
        public ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQuery(long[] fechamentosTurmasDisciplinasIds, string[] alunosCodigos)
        {
            FechamentosTurmasDisciplinasIds = fechamentosTurmasDisciplinasIds;
            AlunosCodigos = alunosCodigos;
        }

        public long[] FechamentosTurmasDisciplinasIds { get; }
        public string[] AlunosCodigos { get; }
    }
}
