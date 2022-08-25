using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorIdTurmaQueryHandler : IRequestHandler<ObterFechamentoTurmaPorIdTurmaQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;

        public ObterFechamentoTurmaPorIdTurmaQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
        }
        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaPorIdTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurma.ObterPorTurma(request.TurmaId, request?.Bimestre);
        }
    }
}