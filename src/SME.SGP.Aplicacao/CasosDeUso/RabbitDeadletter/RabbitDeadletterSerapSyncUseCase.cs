using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RabbitDeadletterSerapSyncUseCase : IRabbitDeadletterSerapSyncUseCase
    {
        private readonly IMediator mediator;

        public RabbitDeadletterSerapSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
        {
            return await mediator.Send(new PublicarFilaSerapEstudantesCommand(RotasRabbitSerapEstudantes.FilaDeadletterSync, string.Empty));
        }
    }
}