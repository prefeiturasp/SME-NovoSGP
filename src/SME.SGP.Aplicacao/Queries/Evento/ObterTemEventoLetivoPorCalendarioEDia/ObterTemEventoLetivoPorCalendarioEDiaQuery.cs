using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTemEventoLetivoPorCalendarioEDiaQuery : IRequest<bool>
    {
        public ObterTemEventoLetivoPorCalendarioEDiaQuery(long tipoCalendarioId, DateTime dataParaVerificar, string dreCodigo, string ueCodigo)
        {
            TipoCalendarioId = tipoCalendarioId;
            DataParaVerificar = dataParaVerificar;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
        }

        public long TipoCalendarioId { get; internal set; }
        public DateTime DataParaVerificar { get; internal set; }
        public string DreCodigo { get; internal set; }
        public string UeCodigo { get; internal set; }
    }
}
