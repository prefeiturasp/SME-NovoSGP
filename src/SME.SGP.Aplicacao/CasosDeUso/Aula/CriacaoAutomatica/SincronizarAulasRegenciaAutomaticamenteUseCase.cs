using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarAulasRegenciaAutomaticamenteUseCase : ISincronizarAulasRegenciaAutomaticamenteUseCase
    {
        private readonly IMediator mediator;

        public SincronizarAulasRegenciaAutomaticamenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var comando = mensagemRabbit.ObterObjetoMensagem<CriarAulasRegenciaAutomaticamenteCommand>();
            if (comando != null)
            {
                await mediator.Send(comando);
                return true;
            }
            return false;
        }
    }
}
