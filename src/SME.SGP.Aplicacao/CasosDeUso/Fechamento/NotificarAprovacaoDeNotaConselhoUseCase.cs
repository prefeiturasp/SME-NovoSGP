using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Fechamento
{
    public class NotificarAprovacaoDeNotaConselhoUseCase : AbstractUseCase, INotificarAprovacaoDeNotaConselhoUseCase
    {
        public NotificarAprovacaoDeNotaConselhoUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await this.mediator.Send(new AprovacaoNotaConselhoClasseCommand());

            return true;
        }
    }
}
