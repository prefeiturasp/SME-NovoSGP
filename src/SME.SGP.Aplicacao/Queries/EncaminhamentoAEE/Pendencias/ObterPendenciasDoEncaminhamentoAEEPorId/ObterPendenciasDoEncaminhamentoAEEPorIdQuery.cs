using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDoEncaminhamentoAEEPorIdQuery : IRequest<IEnumerable<PendenciaEncaminhamentoAEE>>
    {
        public ObterPendenciasDoEncaminhamentoAEEPorIdQuery(long encaminhamentoAEEId)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
        }

        public long EncaminhamentoAEEId { get; set; }
    }
}
