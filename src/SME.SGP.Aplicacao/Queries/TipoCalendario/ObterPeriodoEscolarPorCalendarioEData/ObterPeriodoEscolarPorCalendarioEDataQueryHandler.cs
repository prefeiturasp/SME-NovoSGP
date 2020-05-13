using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorCalendarioEDataQueryHandler : IRequestHandler<ObterPeriodoEscolarPorCalendarioEDataQuery, PeriodoEscolar>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterPeriodoEscolarPorCalendarioEDataQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<PeriodoEscolar> Handle(ObterPeriodoEscolarPorCalendarioEDataQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ObterPeriodoEscolarPorCalendarioEData(request.TipoCalendarioId, request.DataParaVerificar);
        }
    }
}
