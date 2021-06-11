using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConsolidadoPorTurmaBimestreQuery : IRequest<IEnumerable<FechamentoConsolidadoComponenteTurma>>
    {
        public ObterFechamentoConsolidadoPorTurmaBimestreQuery(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }

        public int Bimestre { get; set; }
    }
}
