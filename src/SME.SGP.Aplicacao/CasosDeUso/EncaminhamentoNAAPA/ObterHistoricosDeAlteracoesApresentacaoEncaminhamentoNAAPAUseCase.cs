using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase : IObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAHistoricoDeAlteracaoDto>> Executar(long param)
        {
            return await mediator.Send(new ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery(param));
        }
    }
}
