using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReiniciarSenhaUseCase : IReiniciarSenhaUseCase
    {
        private readonly IMediator mediator;

        public ReiniciarSenhaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf, string dreCodigo, string ueCodigo)
        {
            return await mediator.Send(new ReiniciarSenhaCommand(codigoRf, dreCodigo, ueCodigo));
        }
    }
}
