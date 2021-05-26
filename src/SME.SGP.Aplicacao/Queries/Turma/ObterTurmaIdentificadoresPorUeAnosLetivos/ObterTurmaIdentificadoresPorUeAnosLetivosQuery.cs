using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaIdentificadoresPorUeAnosLetivosQuery : IRequest<IEnumerable<IdentificadoresTurmaDto>>
    {
        public ObterTurmaIdentificadoresPorUeAnosLetivosQuery(long ueId, IEnumerable<int> anosLetivos)
        {
            UeId = ueId;
            AnosLetivos = anosLetivos;
        }

        public long UeId { get; set; }
        public IEnumerable<int> AnosLetivos { get; set; }
    }
}
