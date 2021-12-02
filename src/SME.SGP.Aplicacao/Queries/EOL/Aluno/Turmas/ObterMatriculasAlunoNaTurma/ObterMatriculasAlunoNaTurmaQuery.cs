using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasAlunoNaTurmaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {    
        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }

        public ObterMatriculasAlunoNaTurmaQuery(string codigoTurma, string codigoAluno)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
        }
    }
}
