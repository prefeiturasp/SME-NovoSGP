using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaCommand : IRequest<bool>
    {
        public ExcluirOcorrenciaCommand(IEnumerable<long> ids)
        {
            Ids = ids;
        }

        public IEnumerable<long> Ids { get; set; }
    }
}
