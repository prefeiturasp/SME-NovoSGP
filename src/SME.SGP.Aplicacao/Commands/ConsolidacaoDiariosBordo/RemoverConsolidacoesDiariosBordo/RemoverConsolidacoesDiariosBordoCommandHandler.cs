using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverConsolidacoesDiariosBordoCommandHandler : IRequestHandler<RemoverConsolidacoesDiariosBordoCommand>
    {
        private readonly IRepositorioConsolidacaoDiariosBordo repositorio;

        public RemoverConsolidacoesDiariosBordoCommandHandler(IRepositorioConsolidacaoDiariosBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task Handle(RemoverConsolidacoesDiariosBordoCommand request, CancellationToken cancellationToken)
        {
            await repositorio.ExcluirPorAno(request.AnoLetivo);
        }
    }
}
