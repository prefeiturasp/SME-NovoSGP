using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class UsuarioPossuiAbrangenciaAdmUseCase : AbstractUseCase, IUsuarioPossuiAbrangenciaAdmUseCase
    {
        public UsuarioPossuiAbrangenciaAdmUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<bool> Executar()
        {
            var usuarioId = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);
            var usuarioAdm = await mediator.Send(new ObterUsuarioPossuiAbrangenciaAdmQuery(usuarioId));

            return usuarioAdm;
        }
    }
}
