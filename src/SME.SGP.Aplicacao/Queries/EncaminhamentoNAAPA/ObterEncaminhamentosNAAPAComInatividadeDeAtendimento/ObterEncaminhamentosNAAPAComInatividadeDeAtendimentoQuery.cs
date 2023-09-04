using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery : IRequest<IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>>
    {
        public ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }
}
