using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryHandler : IRequestHandler<ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery, int>
    {
        private readonly IRepositorioAula repositorioAula;

        public ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula;
        }

        public async Task<int> Handle(ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(request.TurmaId, request.ComponenteCurricularId, request.TipoCalendarioId, request.PeriodosEscolaresIds);
    }
}