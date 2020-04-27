using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasQuery : IRequest<IEnumerable<Aula>>
    {
        public string UeCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; internal set; }
        public int Mes { get; set; }
        public string CriadorRF { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
