using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class ListarEventosPorCalendarioRetornoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string TipoEvento { get; set; }
    }
}
