using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase : AbstractUseCase,  IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase
    {
        public ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>> Executar(long param)
        {
            return await mediator.Send(new ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery(param));
        }
    }
}
