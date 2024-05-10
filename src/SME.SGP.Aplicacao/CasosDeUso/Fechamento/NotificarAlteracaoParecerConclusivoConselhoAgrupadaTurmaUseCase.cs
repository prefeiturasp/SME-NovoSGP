using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCase : AbstractUseCase, INotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCase
    {
        public NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await this.mediator.Send(new NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand());

            return true;
        }
    }
}
