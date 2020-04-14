using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FechamentoNotaAlunoDto
    {
        public FechamentoNotaAlunoDto(int bimestre, string notaParaAdicionar, long disciplinaId, string alunoCodigo)
        {
            Bimestre = bimestre;
            NotaConceito = notaParaAdicionar;
            DisciplinaId = disciplinaId;
            AlunoCodigo = alunoCodigo;
        }

        public int Bimestre { get; set; }
        public string NotaConceito { get; set; }
        public long DisciplinaId { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
