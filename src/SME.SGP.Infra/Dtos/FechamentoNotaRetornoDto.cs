using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FechamentoNotaRetornoDto
    {
        public long DisciplinaId { get; set; }
        public string Disciplina { get; set; }
        public double? NotaConceito { get; set; }
        public bool ehConceito { get; set; }
        public string conceitoDescricao { get; set; }
    }
}
