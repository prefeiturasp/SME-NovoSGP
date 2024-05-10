using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase : AbstractUseCase,  IObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase
    {
        public ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAHistoricoDeAlteracaoDto>> Executar(long param)
        {
            return await mediator.Send(new ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery(param));
        }
    }
}
