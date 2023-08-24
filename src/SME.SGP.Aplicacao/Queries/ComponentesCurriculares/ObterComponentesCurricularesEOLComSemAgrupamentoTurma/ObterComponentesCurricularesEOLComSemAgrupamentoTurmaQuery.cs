using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQuery(long[] ids, string codigoTurma = null, bool semAgrupamentoTurma = false)
        {
            SemAgrupamentoTurma = semAgrupamentoTurma;
            CodigoTurma = codigoTurma;
            Ids = ids;
        }

        public bool SemAgrupamentoTurma { get; set; }
        public string CodigoTurma { get; set; }
        public long[] Ids { get; set; }
    }
}
