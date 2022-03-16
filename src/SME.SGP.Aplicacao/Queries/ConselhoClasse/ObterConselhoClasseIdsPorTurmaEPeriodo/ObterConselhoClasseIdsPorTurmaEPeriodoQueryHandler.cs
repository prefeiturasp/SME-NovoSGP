using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseIdsPorTurmaEPeriodoQueryHandler : IRequestHandler<ObterConselhoClasseIdsPorTurmaEPeriodoQuery, long[]>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;

        public ObterConselhoClasseIdsPorTurmaEPeriodoQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
        }
        public async Task<long[]> Handle(ObterConselhoClasseIdsPorTurmaEPeriodoQuery request, CancellationToken cancellationToken)
        {
            var ids = await repositorioConselhoClasseConsulta.ObterConselhoClasseIdsPorTurmaEPeriodoAsync(request.TurmasCodigos, request.PeriodoEscolarId);
            if (ids != null && ids.Any())
                return ids.ToArray();
            return default;
        }
    }
}
