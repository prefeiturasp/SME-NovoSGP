using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutaSincronizacaoEstruturaOranizacionalUesUseCase : AbstractUseCase, IExecutaSincronizacaoEstruturaOranizacionalUesUseCase
    {
        public ExecutaSincronizacaoEstruturaOranizacionalUesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaSincronizacaoEstruturaOranizacionalUesUseCase", "Rabbit - ExecutaSincronizacaoEstruturaOranizacionalUesUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalUes, null, Guid.NewGuid(), null, fila: RotasRabbit.FilaSincronizacaoInstitucional));
        }
    }
}
