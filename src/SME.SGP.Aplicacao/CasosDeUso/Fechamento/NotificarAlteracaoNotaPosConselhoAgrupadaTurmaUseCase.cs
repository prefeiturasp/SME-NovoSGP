using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Fechamento
{
    public class NotificarAlteracaoNotaPosConselhoAgrupadaTurmaUseCase : AbstractUseCase, INotificarAlteracaoNotaPosConselhoAgrupadaTurmaUseCase
    {
        public NotificarAlteracaoNotaPosConselhoAgrupadaTurmaUseCase(IMediator mediator) : base(mediator)
        { 
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await this.mediator.Send(new AprovacaoNotaConselhoClasseCommand());

            return true;
        }
    }
}
