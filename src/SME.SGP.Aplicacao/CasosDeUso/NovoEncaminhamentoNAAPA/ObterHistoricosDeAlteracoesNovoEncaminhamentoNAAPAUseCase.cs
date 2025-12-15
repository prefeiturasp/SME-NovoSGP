using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAUseCase : AbstractUseCase,  IObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAUseCase
    {
        public ObterHistoricosDeAlteracoesNovoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAHistoricoDeAlteracaoDto>> Executar(long param)
        {
            return await mediator.Send(new ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQuery(param));
        }
    }
}
