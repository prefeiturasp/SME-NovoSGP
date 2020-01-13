using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaDto
    {
        public string TurmaId { get; set; }
        public int Bimestre { get; set; }
        public string DisciplinaId { get; set; }
        public string Atividade { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<string> DisciplinasRegenciaIds { get; set; }
        public IEnumerable<CompensacaoAusenciaAlunoDto> Alunos { get; set; }
    }
}
