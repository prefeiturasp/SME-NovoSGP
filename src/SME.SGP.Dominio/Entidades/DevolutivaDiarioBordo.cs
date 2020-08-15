using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class DevolutivaDiarioBordo: EntidadeBase
    {
        public DevolutivaDiarioBordo()
        {
            DiariosBordo = new List<DiarioBordo>();
        }

        public string Descricao { get; set; }
        public List<DiarioBordo> DiariosBordo { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
    }
}
