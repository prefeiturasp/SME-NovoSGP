using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioDinamicoObterEncaminhamentoNAAPAUseCase : AbstractUseCase, IRelatorioDinamicoObterEncaminhamentoNAAPAUseCase
    {
        public RelatorioDinamicoObterEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<RelatorioDinamicoNAAPADto> Executar(FiltroRelatorioDinamicoNAAPADto param)
        {
            return mediator.Send(new ObterEncaminhamentoNAAPAParaRelatorioDinamicoQuery(param));
        }
    }
}
