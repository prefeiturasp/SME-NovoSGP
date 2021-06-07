using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaPendenciasProfessorAvaliacaoUseCase : AbstractUseCase, IExecutaPendenciasProfessorAvaliacaoUseCase
    {
        public ExecutaPendenciasProfessorAvaliacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            SentrySdk.AddBreadcrumb($"Mensagem PendenciasProfessorUseCase", "Rabbit - PendenciasProfessorUseCase");

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaVerificacaoPendenciasProfessor, null, Guid.NewGuid(), null));
        }
    }
}
