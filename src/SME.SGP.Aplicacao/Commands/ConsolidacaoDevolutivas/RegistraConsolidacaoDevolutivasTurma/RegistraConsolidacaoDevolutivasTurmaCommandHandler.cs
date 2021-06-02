using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoDevolutivasTurmaCommandHandler : IRequestHandler<RegistraConsolidacaoDevolutivasTurmaCommand, long>
    {
        private readonly IRepositorioConsolidacaoDevolutivas repositorio;

        public RegistraConsolidacaoDevolutivasTurmaCommandHandler(IRepositorioConsolidacaoDevolutivas repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(RegistraConsolidacaoDevolutivasTurmaCommand request, CancellationToken cancellationToken)
            => await repositorio.Inserir(new ConsolidacaoDevolutivas(request.TurmaId, request.QuantidadeEstimadaDevolutivas, request.QuantidadeRegistradaDevolutivas));
    }
}
