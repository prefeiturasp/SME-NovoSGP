using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaAlunoPorCodigoAlunoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTurmaAlunoPorCodigoAlunoQuery(string codidoAluno)
        {
            CodidoAluno = codidoAluno;
        }

        public string CodidoAluno { get; }
    }
}
