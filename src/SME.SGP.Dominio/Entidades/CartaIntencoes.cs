using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class CartaIntencoes : EntidadeBase
    {
        public long TurmaId { get; set; }

        public Turma Turma { get; set; }

        public long PeriodoEscolarId { get; set; }

        public PeriodoEscolar PeriodoEscolar { get; set; }

        public long ComponenteCurricularId { get; set; }

        public string Planejamento { get; set; }

        public bool Excluido { get; set; }

        public void AdicionarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            PeriodoEscolar = periodoEscolar;
        }
    }
}
