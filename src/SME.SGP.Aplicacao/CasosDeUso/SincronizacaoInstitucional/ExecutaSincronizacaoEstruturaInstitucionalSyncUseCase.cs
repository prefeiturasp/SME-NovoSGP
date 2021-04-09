using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutaSincronizacaoEstruturaInstitucionalSyncUseCase : AbstractUseCase, IExecutaSincronizacaoEstruturaInstitucionalSyncUseCase
    {
        public ExecutaSincronizacaoEstruturaInstitucionalSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem IExecutaSincronizacaoEstruturaInstitucionalSyncUseCase", "Rabbit - ExecutaSincronizacaoEstruturaInstitucionalUesUseCase");

            var codigosDre = await mediator.Send(new ObterCodigosDresQuery());

            foreach (var codigoDre in codigosDre)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, codigoDre, Guid.NewGuid(), null, fila: RotasRabbit.SincronizaEstruturaInstitucionalDreTratar));
            }
        }
    }
}
