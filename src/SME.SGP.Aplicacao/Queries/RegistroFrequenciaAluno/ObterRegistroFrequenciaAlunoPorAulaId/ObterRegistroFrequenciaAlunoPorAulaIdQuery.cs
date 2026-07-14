using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaAlunoPorAulaIdQuery : IRequest<IEnumerable<RegistroFrequenciaAluno>>
    {
        public long AulaId { get; set; }

        public ObterRegistroFrequenciaAlunoPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }
    }
}
