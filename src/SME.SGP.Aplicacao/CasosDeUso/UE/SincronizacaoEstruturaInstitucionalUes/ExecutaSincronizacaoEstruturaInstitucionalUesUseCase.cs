using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutaSincronizacaoEstruturaInstitucionalUesUseCase : AbstractUseCase, IExecutaSincronizacaoEstruturaInstitucionalUesUseCase
    {
        public ExecutaSincronizacaoEstruturaInstitucionalUesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem ExecutaSincronizacaoEstruturaInstitucionalUesUseCase", "Rabbit - ExecutaSincronizacaoEstruturaInstitucionalUesUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalUes, null, Guid.NewGuid(), null, fila: RotasRabbit.FilaSincronizacaoInstitucional));
        }
    }
}
