using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasConsolidacaoFechamentoGeralQueryHandler : IRequestHandler<ObterTurmasConsolidacaoFechamentoGeralQuery, IEnumerable<TurmaConsolidacaoFechamentoGeralDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmasConsolidacaoFechamentoGeralQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>> Handle(ObterTurmasConsolidacaoFechamentoGeralQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasConsolidacaoFechamentoGeralAsync(request.TurmaCodigo);
        }
    }
}
