using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTodosAlunosNaTurmaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTodosAlunosNaTurmaQuery(int codigoTurma, int? codigoAluno = null)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno= codigoAluno;
        }

        public int CodigoTurma { get; set; }
        public int? CodigoAluno { get; set; }
    }
}
