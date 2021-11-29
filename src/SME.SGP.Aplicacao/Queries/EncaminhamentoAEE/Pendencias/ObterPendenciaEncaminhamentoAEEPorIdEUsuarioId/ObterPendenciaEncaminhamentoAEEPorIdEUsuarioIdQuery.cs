using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery : IRequest<PendenciaEncaminhamentoAEE>
    {
        public ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(long encaminhamentoAEEId)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
        }

        public long EncaminhamentoAEEId { get; set; }
    }
}
