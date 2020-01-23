using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroCompensacoesAusenciaDto
    {
        public string TurmaId { get; set; }

        public string DisciplinaId { get; set; }

        public int Bimestre { get; set; }

        public string AtividadeNome { get; set; }

        public string AlunoNome { get; set; }
    }
}
