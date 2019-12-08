using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class AulaPrevista : EntidadeBase
    {
        public int Quantidade { get; set; }

        public int Bimestre { get; set; }

        public long TipoCalendarioId { get; set; }

        public string DisciplinaId { get; set; }

        public string TurmaId { get; set; }
    }
}

