using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaEncerramentoPlanoAEEEstudantesInativosUseCase : AbstractUseCase, IExecutaEncerramentoPlanoAEEEstudantesInativosUseCase
    {
        public ExecutaEncerramentoPlanoAEEEstudantesInativosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem EncerrarPlanoAEEEstudantesInativosUseCase", "Rabbit - EncerrarPlanoAEEEstudantesInativosUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.EncerrarPlanoAEEEstudantesInativos, null, Guid.NewGuid(), null));
        }
    }
}
