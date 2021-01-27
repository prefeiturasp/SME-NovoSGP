using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEAnoLetivoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public string CodigoTurma { get; set; }

        public ObterAlunosPorTurmaEAnoLetivoQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }
    }
}
