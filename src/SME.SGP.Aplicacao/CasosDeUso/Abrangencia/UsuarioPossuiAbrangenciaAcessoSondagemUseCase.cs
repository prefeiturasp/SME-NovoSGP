using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class UsuarioPossuiAbrangenciaAcessoSondagemUseCase : IUsuarioPossuiAbrangenciaAcessoSondagemUseCase
    {
        private readonly IMediator mediator;

        public UsuarioPossuiAbrangenciaAcessoSondagemUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(string usuarioRF, Guid usuarioPerfil)
        {
            return await mediator.Send(new ObterUsuarioPossuiAbrangenciaAcessoSondagemQuery(usuarioRF, usuarioPerfil));
        }
    }
}
