using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQueryHandler : IRequestHandler<ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery, IEnumerable<PeriodoFechamentoBimestre>>
    {
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamentoConsulta;

        public ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQueryHandler(IRepositorioEventoFechamentoConsulta repositorioEventoFechamentoConsulta)
        {
            this.repositorioEventoFechamentoConsulta = repositorioEventoFechamentoConsulta ?? throw new ArgumentNullException(nameof(repositorioEventoFechamentoConsulta));
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> Handle(ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioEventoFechamentoConsulta.ObterPeriodosFechamentoTurmaInfantil(request.TipoCalendarioId, request.Bimestre);
    }
}
