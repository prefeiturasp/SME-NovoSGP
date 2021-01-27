using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PendenciaProfessor.ExcluirPendenciaProfessor
{
    public class ExcluirPendenciaFechamentoCommandHandler : IRequestHandler<ExcluirPendenciaFechamentoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ExcluirPendenciaFechamentoCommandHandler(IMediator mediator, IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<bool> Handle(ExcluirPendenciaFechamentoCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaFechamento.RemoverAsync(request.PendenciaFechamento);

            if (!await mediator.Send(new ExistePendenciaFechamentoPorPendenciaIdQuery(request.PendenciaFechamento.PendenciaId)))
                await mediator.Send(new ExcluirPendenciaPorIdCommand(request.PendenciaFechamento.PendenciaId));

            return true;
        }
    }
}
