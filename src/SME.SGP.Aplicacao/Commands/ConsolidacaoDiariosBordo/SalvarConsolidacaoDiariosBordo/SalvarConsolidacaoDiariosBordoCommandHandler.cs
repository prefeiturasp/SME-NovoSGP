using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoDiariosBordoCommandHandler : AsyncRequestHandler<SalvarConsolidacaoDiariosBordoCommand>
    {
        private readonly IRepositorioConsolidacaoDiariosBordo repositorio;

        public SalvarConsolidacaoDiariosBordoCommandHandler(IRepositorioConsolidacaoDiariosBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        protected override async Task Handle(SalvarConsolidacaoDiariosBordoCommand request, CancellationToken cancellationToken)
        {
            await repositorio.Salvar(request.ConsolidacaoDiariosBordo);
        }
    }
}
