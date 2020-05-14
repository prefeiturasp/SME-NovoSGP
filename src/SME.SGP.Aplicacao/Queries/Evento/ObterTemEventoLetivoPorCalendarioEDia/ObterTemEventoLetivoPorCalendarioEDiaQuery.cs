using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTemEventoLetivoPorCalendarioEDiaQuery : IRequest<bool>
    {
        public long TipoCalendarioId { get; internal set; }
        public DateTime DataParaVerificar { get; internal set; }
        public string DreCodigo { get; internal set; }
        public string UeCodigo { get; internal set; }
    }
}
