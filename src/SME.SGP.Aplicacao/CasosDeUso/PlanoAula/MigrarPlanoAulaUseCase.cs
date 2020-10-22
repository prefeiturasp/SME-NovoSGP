using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanoAulaUseCase : AbstractUseCase, IMigrarPlanoAulaUseCase
    {
        public MigrarPlanoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MigrarPlanoAulaDto param)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            
            return await mediator.Send(new MigrarPlanoAulaCommand(param, usuario));
        }
    }
}
