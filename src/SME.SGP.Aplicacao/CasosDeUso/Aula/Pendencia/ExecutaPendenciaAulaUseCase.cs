using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaPendenciaAulaUseCase : IExecutaPendenciaAulaUseCase
    {
        private readonly IMediator mediator;
        
        public ExecutaPendenciaAulaUseCase(IMediator mediator, IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaPendenciaAulaUseCase", "Rabbit - ExecutaPendenciaAulaUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAula, new PendenciaAulaUseCase(mediator), Guid.NewGuid(), null));
        }
    }
}
