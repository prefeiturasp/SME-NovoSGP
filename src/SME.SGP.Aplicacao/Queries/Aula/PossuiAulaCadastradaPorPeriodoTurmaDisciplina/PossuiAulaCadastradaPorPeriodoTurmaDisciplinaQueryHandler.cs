using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQueryHandler : IRequestHandler<PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQuery, bool>
    {
        private readonly IRepositorioAulaConsulta repositorioAulaConsulta;

        public PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQueryHandler(IRepositorioAulaConsulta repositorioAulaConsulta)
        {
            this.repositorioAulaConsulta = repositorioAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioAulaConsulta));
        }

        public Task<bool> Handle(PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return repositorioAulaConsulta.ExisteAulaNoPeriodoTurmaDisciplinaAsync(request.PeriodoInicio, request.PeriodoFim, request.TurmaCodigo, request.DisciplinaId);
        }
    }
}
