using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirImagemUseCase : IComprimirImagensUseCase
    {
        private readonly IMediator mediator;
        
        public ComprimirImagemUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            return await mediator.Send(new ComprimirImagemCommand(mensagem.Mensagem.ToString()));
        }
    }
}
