using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncaminhamentoAEECommand : IRequest<bool>
    {
        public long EncaminhamentoAEEId { get; set; }

        public SituacaoAEE Situacao { get; set; }

        public GerarPendenciaCPEncaminhamentoAEECommand(long encaminhamentoAEEId, SituacaoAEE situacao)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            Situacao = situacao;
        }
    }

}
