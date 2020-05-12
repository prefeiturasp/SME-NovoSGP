using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosDaUeSMEPorMesQuery : IRequest<IEnumerable<Evento>>
    {
        public string UeCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; internal set; }
        public int Mes { get; set; }
    }
}
