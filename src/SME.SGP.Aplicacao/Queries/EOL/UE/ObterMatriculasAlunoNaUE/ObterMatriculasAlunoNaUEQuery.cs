using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasAlunoNaUEQuery : IRequest<IEnumerable<AlunoPorUeDto>>
    {
        public ObterMatriculasAlunoNaUEQuery(string ueCodigo, string alunoCodigo)
        {
            UeCodigo = ueCodigo;
            AlunoCodigo = alunoCodigo;
        }

        public string UeCodigo { get; }
        public string AlunoCodigo { get; }
    }
}
