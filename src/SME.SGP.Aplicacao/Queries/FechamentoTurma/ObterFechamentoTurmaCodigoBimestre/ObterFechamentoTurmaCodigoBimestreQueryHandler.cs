using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaCodigoBimestreQueryHandler : IRequestHandler<ObterFechamentoTurmaCodigoBimestreQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;

        public ObterFechamentoTurmaCodigoBimestreQueryHandler(IRepositorioFechamentoTurma repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }

        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaCodigoBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.ObterPorTurmaCodigoBimestreAsync(request.TurmaCodigo, request.Bimestre);
        }

    }
}
