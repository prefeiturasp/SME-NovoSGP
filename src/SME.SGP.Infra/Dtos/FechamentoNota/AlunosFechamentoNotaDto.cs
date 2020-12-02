using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunosFechamentoNotaDto
    {
        public long ComponenteCurricularId { get; set; }
        public string ComponenteCurricularDescricao { get; set; }
        public long Nota { get; set; }
        public string NotaConceito { get; set; }
        public bool NotaConceitoAprovado { get; set; }
        public string ProfessorRf { get; set; }
        public string ProfessorNome { get; set; }
        public bool EhConceito { get; set; }
        public string AlunoCodigo { get; set; }
        public string Justificativa { get; set; }
        public long TurmaId { get; set; }
        public long UeId { get; set; }
        public long Bimestre { get; set; }
    }
}
