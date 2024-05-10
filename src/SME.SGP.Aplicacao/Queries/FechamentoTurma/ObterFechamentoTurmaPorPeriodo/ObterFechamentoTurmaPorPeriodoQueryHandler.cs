using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorPeriodoQueryHandler : IRequestHandler<ObterFechamentoTurmaPorPeriodoQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;

        public ObterFechamentoTurmaPorPeriodoQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaPorPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.ObterPorTurmaPeriodo(request.TurmaId, request.PeriodoId);
        }

    }
}
