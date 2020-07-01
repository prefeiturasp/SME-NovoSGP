using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            var usuarioAdm = await mediator.Send(new ObterUsuarioPossuiAbrangenciaAdmQuery(usuarioId));

            return usuarioAdm;
        }
    }
}
