using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQueryHandler : IRequestHandler<ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery, long[]>
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequenciaConsulta;

        public ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQueryHandler(IRepositorioFrequenciaConsulta repositorioFrequenciaConsulta)
        {
            this.repositorioFrequenciaConsulta = repositorioFrequenciaConsulta ?? throw new ArgumentNullException(nameof(repositorioFrequenciaConsulta));
        }

        public async Task<long[]> Handle(ObterFrequenciasAlunoIdsComPresencasMaiorQueTotalAulasPorUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaConsulta
                .ObterFrequenciasAlunosIdsComPresencasMaiorTotalAulas(request.UeId, request.AnoLetivo);
        }
    }
}
