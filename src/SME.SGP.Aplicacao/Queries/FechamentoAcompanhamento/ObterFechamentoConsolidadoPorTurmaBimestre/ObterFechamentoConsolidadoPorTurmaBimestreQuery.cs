using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConsolidadoPorTurmaBimestreQuery : IRequest<IEnumerable<FechamentoConsolidadoComponenteTurma>>
    {
        public ObterFechamentoConsolidadoPorTurmaBimestreQuery(long turmaId, int bimestre, int[] situacoesFechamento)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            SituacoesFechamento = situacoesFechamento;
        }

        public long TurmaId { get; set; }

        public int Bimestre { get; set; }
        public int[] SituacoesFechamento { get; set; }
    }
}
