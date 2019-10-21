using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dto
{
    public class FiltroEventoTipoDto
    {
        public EventoLocalOcorrencia LocalOcorrencia { get; set; }
        public EventoLetivo Letivo { get; set; }
        public string Descricao { get; set; }
    }
}
