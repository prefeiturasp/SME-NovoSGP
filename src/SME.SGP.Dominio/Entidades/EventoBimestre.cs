using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class EventoBimestre : EntidadeBase
    {
        public long EventoId { get; set; }
        public int? Bimestre { get; set; }
    }
}
