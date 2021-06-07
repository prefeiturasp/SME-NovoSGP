using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaPendenciasAusenciaFechamentoUseCase : AbstractUseCase, IExecutaPendenciasAusenciaFechamentoUseCase
    {
        public ExecutaPendenciasAusenciaFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem PendenciasAusenciaFechamentoUseCase", "Rabbit - PendenciasAusenciaFechamentoUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaVerificacaoPendenciasAusenciaFechamento, null, Guid.NewGuid(), null));
        }
    }
}
