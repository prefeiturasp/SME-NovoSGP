using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQueryHandler : IRequestHandler<ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQuery, IEnumerable<TipoCalendarioAulasAutomaticasDto>>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<IEnumerable<TipoCalendarioAulasAutomaticasDto>> Handle(ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ObterTiposCalendariosAulaAutomaticaPorAnoLetivosEModalidadesAsync(request.AnoLetivo, request.Modalidades);
        }
    }
}
