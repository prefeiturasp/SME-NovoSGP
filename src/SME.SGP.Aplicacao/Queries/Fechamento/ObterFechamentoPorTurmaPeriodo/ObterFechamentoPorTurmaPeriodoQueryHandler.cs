using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPorTurmaPeriodoQueryHandler : IRequestHandler<ObterFechamentoPorTurmaPeriodoQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;

        public ObterFechamentoPorTurmaPeriodoQueryHandler(IRepositorioFechamentoTurma repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<FechamentoTurma> Handle(ObterFechamentoPorTurmaPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.ObterPorTurmaPeriodo(request.TurmaId, request.PeriodoEscolarId);            
        }
    }
}
