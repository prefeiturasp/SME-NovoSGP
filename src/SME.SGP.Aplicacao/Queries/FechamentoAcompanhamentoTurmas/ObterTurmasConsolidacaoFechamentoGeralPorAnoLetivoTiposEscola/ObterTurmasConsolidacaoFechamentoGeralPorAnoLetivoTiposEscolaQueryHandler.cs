using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasConsolidacaoFechamentoGeralPorAnoLetivoTiposEscolaQueryHandler : IRequestHandler<ObterTurmasConsolidacaoFechamentoGeralPorAnoLetivoTiposEscolaQuery, IEnumerable<TurmaConsolidacaoFechamentoGeralDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmasConsolidacaoFechamentoGeralPorAnoLetivoTiposEscolaQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>> Handle(ObterTurmasConsolidacaoFechamentoGeralPorAnoLetivoTiposEscolaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasConsolidacaoFechamentoGeralAsync(request.AnoLetivo, request.Pagina, request.QuantidadeRegistrosPorPagina, request.TiposEscola);
        }
    }
}
