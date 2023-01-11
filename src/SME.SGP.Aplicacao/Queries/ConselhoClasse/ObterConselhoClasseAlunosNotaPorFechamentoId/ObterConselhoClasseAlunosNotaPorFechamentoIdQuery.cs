using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunosNotaPorFechamentoIdQuery : IRequest<IEnumerable<ConselhoClasseAlunosNotaPorFechamentoIdDto>>
    {
        public long FechamentoTurmaId { get; set; }

        public ObterConselhoClasseAlunosNotaPorFechamentoIdQuery(long fechamentoTurmaId)
        {
            FechamentoTurmaId = fechamentoTurmaId;
        }
    }
}
