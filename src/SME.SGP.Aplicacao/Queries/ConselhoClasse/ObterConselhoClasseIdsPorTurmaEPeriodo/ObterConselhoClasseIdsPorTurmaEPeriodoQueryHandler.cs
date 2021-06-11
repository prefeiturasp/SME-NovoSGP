using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseIdsPorTurmaEPeriodoQueryHandler : IRequestHandler<ObterConselhoClasseIdsPorTurmaEPeriodoQuery, long[]>
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;

        public ObterConselhoClasseIdsPorTurmaEPeriodoQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasse));
        }
        public async Task<long[]> Handle(ObterConselhoClasseIdsPorTurmaEPeriodoQuery request, CancellationToken cancellationToken)
        {
            var ids = await repositorioConselhoClasse.ObterConselhoClasseIdsPorTurmaEPeriodoAsync(request.TurmasCodigos, request.PeriodoEscolarId);
            if (ids != null && ids.Any())
                return ids.ToArray();
            return default;
        }
    }
}
