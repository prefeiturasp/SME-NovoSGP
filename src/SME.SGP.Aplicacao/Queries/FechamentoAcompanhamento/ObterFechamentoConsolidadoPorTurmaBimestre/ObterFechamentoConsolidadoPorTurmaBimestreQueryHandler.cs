using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConsolidadoPorTurmaBimestreQueryHandler : IRequestHandler<ObterFechamentoConsolidadoPorTurmaBimestreQuery, IEnumerable<FechamentoConsolidadoComponenteTurma>>
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public ObterFechamentoConsolidadoPorTurmaBimestreQueryHandler(IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        public async Task<IEnumerable<FechamentoConsolidadoComponenteTurma>> Handle(ObterFechamentoConsolidadoPorTurmaBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoConsolidado.ObterFechamentosConsolidadoPorTurmaBimestre(request.TurmaId, request.Bimestre);
        }
    }
}
