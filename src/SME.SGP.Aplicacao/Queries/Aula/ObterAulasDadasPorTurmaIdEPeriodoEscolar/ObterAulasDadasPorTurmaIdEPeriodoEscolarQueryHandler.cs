using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmaIdEPeriodoEscolarQueryHandler : IRequestHandler<ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery, int>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulasDadasPorTurmaIdEPeriodoEscolarQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula;
        }

        public async Task<int> Handle(ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery request, CancellationToken cancellationToken)
            => await repositorioAula.ObterAulasDadasPorTurmaEPeriodoEscolar(request.TurmaId, request.TipoCalendarioId, request.PeriodosEscolaresIds);
    }
}