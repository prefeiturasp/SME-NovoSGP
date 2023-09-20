using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarAulasRegenciaAutomaticamenteUseCase : AbstractUseCase, ISincronizarAulasRegenciaAutomaticamenteUseCase
    {
        public SincronizarAulasRegenciaAutomaticamenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var comando = mensagemRabbit.ObterObjetoMensagem<CriarAulasRegenciaAutomaticamenteCommand>();
            if (comando.NaoEhNulo())
            {
                await mediator.Send(comando);
                return true;
            }
            return false;
        }
    }
}
