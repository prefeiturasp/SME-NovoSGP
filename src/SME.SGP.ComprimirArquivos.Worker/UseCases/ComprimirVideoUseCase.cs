using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirVideoUseCase : IComprimirVideoUseCase
    {
        private readonly IMediator mediator;
        
        public ComprimirVideoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            return await mediator.Send(new ComprimirVideoCommand(mensagem.Mensagem.ToString()));
        }
    }
}
