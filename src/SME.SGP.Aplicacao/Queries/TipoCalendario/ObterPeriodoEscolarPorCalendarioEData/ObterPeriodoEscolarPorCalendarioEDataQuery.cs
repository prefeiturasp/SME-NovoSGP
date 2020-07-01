using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorCalendarioEDataQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodoEscolarPorCalendarioEDataQuery(long tipoCalendarioId, DateTime dataParaVerificar)
        {
            TipoCalendarioId = tipoCalendarioId;
            DataParaVerificar = dataParaVerificar;
        }

        public long TipoCalendarioId { get; internal set; }
        public DateTime DataParaVerificar { get; internal set; }
    }
}
