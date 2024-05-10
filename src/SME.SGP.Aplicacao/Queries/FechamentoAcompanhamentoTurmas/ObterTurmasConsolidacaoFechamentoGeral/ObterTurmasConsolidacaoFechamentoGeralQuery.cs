using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasConsolidacaoFechamentoGeralQuery : IRequest<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>>
    {
        public ObterTurmasConsolidacaoFechamentoGeralQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; }
    }
}
