using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{ 
    public class ExecutarSincronizacaoAulasRegenciaAutomaticasUseCase : AbstractUseCase, IExecutarSincronizacaoAulasRegenciaAutomaticasUseCase
    {
        public ExecutarSincronizacaoAulasRegenciaAutomaticasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.CarregarDadosUeTurmaRegenciaAutomaticamente, null, Guid.NewGuid(), null));
        }
    }
}
