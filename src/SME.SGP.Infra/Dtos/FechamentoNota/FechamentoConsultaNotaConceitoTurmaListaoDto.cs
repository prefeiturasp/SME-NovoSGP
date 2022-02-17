using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FechamentoConsultaNotaConceitoTurmaListaoDto
    {
        public int Bimestre { get; set; }
        public string Disciplina { get; set; }
        public long DisciplinaCodigo { get; set; }
        public double? NotaConceito { get; set; }
        public bool EmAprovacao { get; set; }
    }
}
