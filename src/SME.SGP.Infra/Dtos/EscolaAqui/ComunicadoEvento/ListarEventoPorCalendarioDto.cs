using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos.EscolaAqui.ComunicadoEvento
{
    public class ListarEventoPorCalendarioDto
    {
        public int TipoCalendario { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int? Modalidade { get; set; }
    }
}
