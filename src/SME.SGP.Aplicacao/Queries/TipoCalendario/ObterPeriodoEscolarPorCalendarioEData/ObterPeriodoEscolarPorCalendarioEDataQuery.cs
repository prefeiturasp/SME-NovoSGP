using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarPorCalendarioEDataQuery : IRequest<PeriodoEscolar>
    {
        public long TipoCalendarioId { get; internal set; }
        public DateTime DataParaVerificar { get; internal set; }
    }
}
