using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilUseCase : ICriarAulasInfantilUseCase
    {
        private readonly IMediator mediator;

        public CriarAulasInfantilUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var comando = mensagemRabbit.ObterObjetoMensagem<CriarAulasInfantilAutomaticamenteCommand>();
            if (comando != null)
            {
                await mediator.Send(comando);
                return true;
            }
            return false;
        }
    }
}
