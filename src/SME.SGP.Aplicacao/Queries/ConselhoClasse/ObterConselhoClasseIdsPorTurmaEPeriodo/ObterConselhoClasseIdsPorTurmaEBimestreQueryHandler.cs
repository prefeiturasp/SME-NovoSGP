using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseIdsPorTurmaEBimestreQueryHandler : IRequestHandler<ObterConselhoClasseIdsPorTurmaEBimestreQuery, long[]>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;
        public ObterConselhoClasseIdsPorTurmaEBimestreQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta)
        {
            this.repositorioConselhoClasseConsulta = repositorioConselhoClasseConsulta ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsulta));
        }
        public async Task<long[]> Handle(ObterConselhoClasseIdsPorTurmaEBimestreQuery request, CancellationToken cancellationToken)
        {
            var ids = await repositorioConselhoClasseConsulta.ObterConselhoClasseIdsPorTurmaEBimestreAsync(request.TurmasCodigos, request.Bimestre);
            if (ids.NaoEhNulo() && ids.Any())
                return ids.ToArray();
            return default;
        }
    }
}
