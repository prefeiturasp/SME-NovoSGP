using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorIdQueryHandler : IRequestHandler<ObterFechamentoTurmaPorIdQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;

        public ObterFechamentoTurmaPorIdQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.ObterPorFechamentoTurmaIdAsync(request.FechamentoTurmaId);
        }
    }
}
