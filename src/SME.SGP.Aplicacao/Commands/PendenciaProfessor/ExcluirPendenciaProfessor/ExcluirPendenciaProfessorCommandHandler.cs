using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PendenciaProfessor.ExcluirPendenciaProfessor
{
    public class ExcluirPendenciaProfessorCommandHandler : IRequestHandler<ExcluirPendenciaProfessorCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaProfessor repositorioPendenciaProfessor;

        public ExcluirPendenciaProfessorCommandHandler(IMediator mediator, IRepositorioPendenciaProfessor repositorioPendenciaProfessor)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaProfessor = repositorioPendenciaProfessor ?? throw new ArgumentNullException(nameof(repositorioPendenciaProfessor));
        }

        public async Task<bool> Handle(ExcluirPendenciaProfessorCommand request, CancellationToken cancellationToken)
        {
            await repositorioPendenciaProfessor.Remover(request.PendenciaProfessor);

            if (!await mediator.Send(new ExistePendenciaProfessorPorPendenciaIdQuery(request.PendenciaProfessor.PendenciaId)))
                await mediator.Send(new ExcluirPendenciaPorIdCommand(request.PendenciaProfessor.PendenciaId));

            return true;
        }
    }
}
