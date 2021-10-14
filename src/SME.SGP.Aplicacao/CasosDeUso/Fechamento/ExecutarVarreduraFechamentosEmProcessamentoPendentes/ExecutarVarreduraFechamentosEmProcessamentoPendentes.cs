using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVarreduraFechamentosEmProcessamentoPendentes: AbstractUseCase, IExecutarVarreduraFechamentosEmProcessamentoPendentes
    {
        public ExecutarVarreduraFechamentosEmProcessamentoPendentes(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task Executar()
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.VarreduraFechamentosTurmaDisciplinaEmProcessamentoPendentes, string.Empty, Guid.NewGuid(), null));
        }
    }
}
