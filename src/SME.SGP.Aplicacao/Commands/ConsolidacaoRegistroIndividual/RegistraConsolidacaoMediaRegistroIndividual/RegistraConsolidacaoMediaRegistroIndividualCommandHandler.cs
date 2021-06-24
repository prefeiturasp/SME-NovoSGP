using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoMediaRegistroIndividualCommandHandler : IRequestHandler<RegistraConsolidacaoMediaRegistroIndividualCommand, long>
    {
        private readonly IRepositorioConsolidacaoRegistroIndividualMedia repositorio;

        public RegistraConsolidacaoMediaRegistroIndividualCommandHandler(IRepositorioConsolidacaoRegistroIndividualMedia repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(RegistraConsolidacaoMediaRegistroIndividualCommand request, CancellationToken cancellationToken)
        { 
            return await repositorio.Inserir(new ConsolidacaoRegistroIndividualMedia(request.TurmaId, request.Quantidade));
        }
    }
}
