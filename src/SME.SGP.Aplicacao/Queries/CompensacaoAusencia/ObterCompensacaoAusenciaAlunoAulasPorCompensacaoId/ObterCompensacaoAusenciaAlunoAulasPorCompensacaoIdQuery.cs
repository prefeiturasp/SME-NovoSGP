using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery : IRequest<IEnumerable<Dominio.CompensacaoAusenciaAlunoAula>>
    {
        public ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery(long compensacaoId)
        {
            CompensacaoId = compensacaoId;
        }

        public long CompensacaoId { get; }
    }
}
