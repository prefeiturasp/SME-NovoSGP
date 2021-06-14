using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQueryHandler : IRequestHandler<ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery, int>
    {
        private readonly IRepositorioAulaPrevistaBimestre repositorioAulaPrevistaBimestre;

        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQueryHandler(IRepositorioAulaPrevistaBimestre repositorioAulaPrevistaBimestre)
        {
            this.repositorioAulaPrevistaBimestre = repositorioAulaPrevistaBimestre;
        }

        public async Task<int> Handle(ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            var aulaPrevistaBimestre = await repositorioAulaPrevistaBimestre.ObterAulasPrevistasPorTurmaTipoCalendarioDisciplina(request.TipoCalendarioId, request.CodigoTurma, request.ComponenteCurricularId.ToString(), request.Bimestre);
            return aulaPrevistaBimestre?.Sum(x => x.Previstas) ?? default;
        }
    }
}