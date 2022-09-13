using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaCompletoPorIdQueryHandler : IRequestHandler<ObterFechamentoTurmaCompletoPorIdQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;

        public ObterFechamentoTurmaCompletoPorIdQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaCompletoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.ObterCompletoPorIdAsync(request.FechamentoTurmaId);
        }
    }
}