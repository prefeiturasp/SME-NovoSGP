using MediatR;
using SME.SGP.Aplicacao.Queries.PendenciaDiarioBordo;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PendenciaDiarioBordo
{
    public class ExcluirPendenciaDiarioBordoCommandHandler : IRequestHandler<ExcluirPendenciaDiarioBordoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo;
        public ExcluirPendenciaDiarioBordoCommandHandler(IMediator mediator, IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
        }
        public async Task<bool> Handle(ExcluirPendenciaDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var existePendencia = await mediator.Send(new ObterIdPendenciaDiarioBordoQuery(request.PendenciaId), cancellationToken);

            if (existePendencia)
                await repositorioPendenciaDiarioBordo.Excluir(request.PendenciaId);
            return true;

        }
    }
}
