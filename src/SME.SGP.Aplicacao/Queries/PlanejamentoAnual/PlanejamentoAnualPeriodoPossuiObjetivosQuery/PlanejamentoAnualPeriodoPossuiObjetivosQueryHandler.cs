using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PlanejamentoAnualPeriodoPossuiObjetivosQueryHandler : IRequestHandler<PlanejamentoAnualPeriodoPossuiObjetivosQuery, bool>
    {
        private readonly IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar;

        public PlanejamentoAnualPeriodoPossuiObjetivosQueryHandler(IRepositorioPlanejamentoAnualPeriodoEscolar repositorioPlanejamentoAnualPeriodoEscolar)
        {
            this.repositorioPlanejamentoAnualPeriodoEscolar = repositorioPlanejamentoAnualPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPlanejamentoAnualPeriodoEscolar));
        }

        public async Task<bool> Handle(PlanejamentoAnualPeriodoPossuiObjetivosQuery request, CancellationToken cancellationToken)
            => await repositorioPlanejamentoAnualPeriodoEscolar.PlanejamentoPossuiObjetivos(request.PlanejamentoAnualPeriodoId);
    }
}
