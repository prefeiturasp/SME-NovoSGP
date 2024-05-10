using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdQuery : IRequest<PendenciaEncaminhamentoAEE>
    {
        public ObterPendenciaEncaminhamentoAEEPorIdQuery(long encaminhamentoAEEId)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
        }

        public long EncaminhamentoAEEId { get; set; }
    }
}
