using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoFrequenciaTurmaCommandHandler : IRequestHandler<RegistraConsolidacaoFrequenciaTurmaCommand, long>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorio;

        public RegistraConsolidacaoFrequenciaTurmaCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(RegistraConsolidacaoFrequenciaTurmaCommand request, CancellationToken cancellationToken)
            => await repositorio.Inserir(new ConsolidacaoFrequenciaTurma(request.TurmaId, request.QuantidadeAcimaMinimoFrequencia, request.QuantidadeAbaixoMinimoFrequencia));
    }
}
