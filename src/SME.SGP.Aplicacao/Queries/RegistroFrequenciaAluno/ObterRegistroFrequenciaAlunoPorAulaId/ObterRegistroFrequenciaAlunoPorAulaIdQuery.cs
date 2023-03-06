using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
