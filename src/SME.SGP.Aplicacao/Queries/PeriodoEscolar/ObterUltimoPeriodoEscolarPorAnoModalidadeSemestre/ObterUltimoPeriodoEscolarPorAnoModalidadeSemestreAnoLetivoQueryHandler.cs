using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQueryHandler : IRequestHandler<ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery, PeriodoEscolar>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQueryHandler(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<PeriodoEscolar> Handle(ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoEscolar.ObterUltimoBimestreAsync(request.AnoLetivo, request.Modalidade, request.Semestre);
    }
}
