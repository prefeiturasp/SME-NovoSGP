using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery : IRequest<PendenciaEncaminhamentoAEE>
    {
        public ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(long encaminhamentoAEEId, long usuarioId)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            UsuarioId = usuarioId;
        }

        public long EncaminhamentoAEEId { get; set; }

        public long UsuarioId { get; set; }
    }
}
