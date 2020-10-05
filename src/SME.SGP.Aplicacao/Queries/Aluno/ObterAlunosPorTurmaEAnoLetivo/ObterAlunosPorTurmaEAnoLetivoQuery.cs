using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEAnoLetivoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public int AnoLetivo { get; set; }

        public string CodigoTurma { get; set; }

        public ObterAlunosPorTurmaEAnoLetivoQuery(string codigoTurma, int anoLetivo)
        {
            CodigoTurma = codigoTurma;
            AnoLetivo = anoLetivo;
        }
    }
}
