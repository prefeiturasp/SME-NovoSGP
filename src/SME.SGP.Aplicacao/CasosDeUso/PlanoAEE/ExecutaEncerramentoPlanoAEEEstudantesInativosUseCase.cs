using MediatR;
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
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.EncerrarPlanoAEEEstudantesInativos, null, Guid.NewGuid(), null));
        }
    }
}