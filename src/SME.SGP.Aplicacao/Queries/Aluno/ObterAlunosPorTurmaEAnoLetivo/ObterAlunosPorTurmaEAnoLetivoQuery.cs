using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEAnoLetivoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public string CodigoTurma { get; set; }
        public int? AnoLetivo { get; set; }

        public ObterAlunosPorTurmaEAnoLetivoQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public ObterAlunosPorTurmaEAnoLetivoQuery(string codigoTurma, int anoLetivo)
        {
            CodigoTurma = codigoTurma;
            AnoLetivo = anoLetivo;
        }
    }
}
