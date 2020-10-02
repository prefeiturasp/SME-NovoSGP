using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarePorIdQueryHandler : IRequestHandler<ObterPeriodoEscolarePorIdQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ObterPeriodoEscolarePorIdQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }
        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPeriodoEscolar.ObterPorIdAsync(request.Id);
        }
    }
}
