using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase : IObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<GraficoEncaminhamentoNAAPADto> Executar(FiltroQuantidadeEncaminhamentoNAAPAEmAbertoDto param)
        {
            return mediator.Send(new ObterQuantidadeAtendimentoNAAPAEmAbertoQuery(param.AnoLetivo, param.DreId, param.Modalidade));
        }
    }
}
