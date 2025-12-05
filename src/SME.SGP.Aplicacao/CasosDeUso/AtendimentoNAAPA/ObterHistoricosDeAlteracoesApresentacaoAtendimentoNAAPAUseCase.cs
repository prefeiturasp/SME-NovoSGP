using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase : AbstractUseCase,  IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase
    {
        public ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>> Executar(long param)
        {
            return await mediator.Send(new ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQuery(param));
        }
    }
}
