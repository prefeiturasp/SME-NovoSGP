using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesFechamentoConsolidadoPorTurmaBimestreQueryHandler : IRequestHandler<ObterComponentesFechamentoConsolidadoPorTurmaBimestreQuery, IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>>
    {
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;

        public ObterComponentesFechamentoConsolidadoPorTurmaBimestreQueryHandler(IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado)
        {
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
        }

        public async Task<IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>> Handle(ObterComponentesFechamentoConsolidadoPorTurmaBimestreQuery request, CancellationToken cancellationToken)
        {
            var componentes = await repositorioFechamentoConsolidado.ObterComponentesFechamentoConsolidadoPorTurmaBimestre(request.TurmaId, request.Bimestre);
            return componentes;
        }
    }
}
