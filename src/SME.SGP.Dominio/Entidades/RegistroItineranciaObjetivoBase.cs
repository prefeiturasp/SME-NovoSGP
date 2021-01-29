using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class RegistroItineranciaObjetivoBase
    {
        public long Id { get; set; }
        public bool TemDescricao { get; set; }
        public bool PermiteVariasUes { get; set; }
        public bool Excluido { get; set; }
    }
}
