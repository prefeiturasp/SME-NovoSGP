using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDevolutivaCommandHandler : IRequestHandler<ExcluirDevolutivaCommand, bool>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;
        private readonly IMediator mediator;

        public ExcluirDevolutivaCommandHandler(IRepositorioDevolutiva repositorioDevolutiva, IMediator mediator)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirDevolutivaCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExcluirNotificacaoDevolutiva,
                      new ExcluirNotificacaoDevolutivaDto(request.DevolutivaId), Guid.NewGuid(), null));

            await repositorioDevolutiva.RemoverLogico(request.DevolutivaId);

            return true;
        }
    }
}
