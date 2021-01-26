using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PendenciaParametroEventoDto
    {
        public long ParametroSistemaId { get; set; }
        public string Descricao { get; set; }
        public int Valor { get; set; }
    }
}
