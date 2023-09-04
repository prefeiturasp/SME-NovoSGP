using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryHandler : IRequestHandler<ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery, IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>>
    {
        public ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryHandler()
        {
        }

        public Task<IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> Handle(ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
