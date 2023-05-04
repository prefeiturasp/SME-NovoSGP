using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoDevolutivasTurmaCommandHandler : AsyncRequestHandler<RegistraConsolidacaoDevolutivasTurmaCommand>
    {
        private readonly IRepositorioConsolidacaoDevolutivas repositorio;

        public RegistraConsolidacaoDevolutivasTurmaCommandHandler(IRepositorioConsolidacaoDevolutivas repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        protected override async Task Handle(RegistraConsolidacaoDevolutivasTurmaCommand request, CancellationToken cancellationToken)
        { 
            await repositorio.Salvar(request.ConsolidacaoDevolutivas);
        }
    }
}
