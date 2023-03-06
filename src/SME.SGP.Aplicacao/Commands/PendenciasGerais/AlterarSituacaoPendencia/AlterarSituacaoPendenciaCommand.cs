using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoPendenciaCommand : IRequest<bool>
    {
        public AlterarSituacaoPendenciaCommand(long pendenciaId, SituacaoPendencia novaSituacao)
        {
            PendenciaId = pendenciaId;
            NovaSituacao = novaSituacao;
        }

        public long PendenciaId { get; set; }
        public SituacaoPendencia NovaSituacao { get; set; }
    }
}
