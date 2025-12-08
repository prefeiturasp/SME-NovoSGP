using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioDinamicoObterAtendimentoNAAPAUseCase : AbstractUseCase, IRelatorioDinamicoObterAtendimentoNAAPAUseCase
    {
        public RelatorioDinamicoObterAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<RelatorioDinamicoNAAPADto> Executar(FiltroRelatorioDinamicoNAAPADto param)
        {
            return mediator.Send(new ObterAtendimentoNAAPAParaRelatorioDinamicoQuery(param));
        }
    }
}
