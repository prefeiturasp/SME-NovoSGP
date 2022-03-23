using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoConselhoClasseUseCase : AbstractUseCase, IAtualizarSituacaoConselhoClasseUseCase
    {
        public AtualizarSituacaoConselhoClasseUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var command = mensagem.ObterObjetoMensagem<AtualizaSituacaoConselhoClasseCommand>();
                        
            await mediator.Send(command);            

            return true;
        }
    }
}
