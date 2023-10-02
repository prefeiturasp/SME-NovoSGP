using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilERegenciaUseCase : ICriarAulasInfantilERegenciaUseCase
    {
        private readonly IMediator mediator;

        public CriarAulasInfantilERegenciaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {            
            var comando = mensagemRabbit.ObterObjetoMensagem<CriarAulasInfantilERegenciaAutomaticamenteCommand>();           

            if (comando.NaoEhNulo())
            {
                await mediator.Send(comando);
                return true;
            }
            return false;
        }
    }
}
