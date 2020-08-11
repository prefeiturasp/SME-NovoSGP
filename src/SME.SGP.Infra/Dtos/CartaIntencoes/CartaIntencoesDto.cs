using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class CartaIntencoesDto
    {
        public string CodigoTurma { get; set; }
        public long TurmaId { get; set; }

        public TurmaDto Turma { get; set; }

        public long PeriodoEscolarId { get; set; }

        public int Bimestre { get; set; }

        public PeriodoEscolarDto PeriodoEscolar { get; set; }

        public long ComponenteCurricularId { get; set; }

        public string Planejamento { get; set; }

        public bool Excluido { get; set; }
    }
}
