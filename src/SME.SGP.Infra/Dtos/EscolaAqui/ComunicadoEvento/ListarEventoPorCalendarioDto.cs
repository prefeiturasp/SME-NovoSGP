using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ListarEventoPorCalendarioDto
    {
        public int TipoCalendario { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IEnumerable<Modalidade> Modalidades { get; set; }
    }
}
