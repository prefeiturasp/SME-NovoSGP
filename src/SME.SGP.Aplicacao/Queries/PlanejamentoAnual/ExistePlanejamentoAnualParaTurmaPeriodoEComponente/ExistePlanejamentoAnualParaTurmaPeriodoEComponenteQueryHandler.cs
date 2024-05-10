using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQueryHandler : IRequestHandler<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery, long>
    {
        private readonly IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual;

        public ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQueryHandler(IRepositorioPlanejamentoAnual repositorioPlanejamentoAnual)
        {
            this.repositorioPlanejamentoAnual = repositorioPlanejamentoAnual ?? throw new ArgumentNullException(nameof(repositorioPlanejamentoAnual));
        }

        public async Task<long> Handle(ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioPlanejamentoAnual.ExistePlanejamentoAnualParaTurmaPeriodoEComponente(request.TurmaId, request.PeriodoEscolarId, request.ComponenteCurricularId);
    }
}
