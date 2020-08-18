using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class CartaIntencoesDto
    {
        public long Id { get; set; }

        public long PeriodoEscolarId { get; set; }

        public string Planejamento { get; set; }
    }
}
