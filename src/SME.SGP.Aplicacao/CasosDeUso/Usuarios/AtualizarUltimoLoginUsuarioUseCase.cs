using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarUltimoLoginUsuarioUseCase : IAtualizarUltimoLoginUsuarioUseCase
    {
        private readonly IMediator mediator;

        public AtualizarUltimoLoginUsuarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var usuario = param.ObterObjetoMensagem<Usuario>();
            await mediator.Send(new AtualizarUltimoLoginUsuarioCommand(usuario));

            return true;
        }
    }
}
