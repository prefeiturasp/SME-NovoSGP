using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreQueryHandler : IRequestHandler<ObterQuantidadeAulasPrevistasPorTurmaEBimestreQuery, int>
    {
        private readonly IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestre;

        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreQueryHandler(IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestre)
        {
            this.repositorioAulaPrevistaBimestre = repositorioAulaPrevistaBimestre;
        }

        public async Task<int> Handle(ObterQuantidadeAulasPrevistasPorTurmaEBimestreQuery request, CancellationToken cancellationToken)
        {
            var aulaPrevistaBimestre = await repositorioAulaPrevistaBimestre.ObterAulasPrevistasPorTurmaTipoCalendarioBimestre(request.TipoCalendarioId, request.CodigoTurma, request.Bimestre);
            return aulaPrevistaBimestre?.Sum(x => x.Previstas) ?? default;
        }
    }
}