using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorIdsQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorIdsQuery(long[] turmasIds)
        {
            TurmasIds = turmasIds;
        }

        public long[] TurmasIds { get; set; }
    }
}
