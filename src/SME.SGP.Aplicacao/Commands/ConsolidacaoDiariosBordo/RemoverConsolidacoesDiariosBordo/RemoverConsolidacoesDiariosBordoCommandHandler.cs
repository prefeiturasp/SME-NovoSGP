using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverConsolidacoesDiariosBordoCommandHandler : AsyncRequestHandler<RemoverConsolidacoesDiariosBordoCommand>
    {
        private readonly IRepositorioConsolidacaoDiariosBordo repositorio;

        public RemoverConsolidacoesDiariosBordoCommandHandler(IRepositorioConsolidacaoDiariosBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        protected override async Task Handle(RemoverConsolidacoesDiariosBordoCommand request, CancellationToken cancellationToken)
        {
            await repositorio.ExcluirPorAno(request.AnoLetivo);
        }
    }
}
