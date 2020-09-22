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
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;
        public ExecutaPendenciaAulaUseCase(IMediator mediator, IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }
        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaPendenciaAulaUseCase", "Rabbit - ExecutaPendenciaAulaUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaExecutaPendenciasAula, new PendenciaAulaUseCase(repositorioPendenciaAula), Guid.NewGuid(), null));
        }
    }
}
