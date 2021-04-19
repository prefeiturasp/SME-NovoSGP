using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaDto
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "A turma é obrigatória!")]
        public string TurmaId { get; set; }
        
        [Required(ErrorMessage = "O bimestre é obrigatório")]
        public int Bimestre { get; set; }
        
        [Required(ErrorMessage = "O componente curricular é obrigatório!")]
        public string DisciplinaId { get; set; }
        
        [Required(ErrorMessage = "O nome da atividade é obrigatório!")]
        public string Atividade { get; set; }
        
        public string Descricao { get; set; }
        
        public IEnumerable<string> DisciplinasRegenciaIds { get; set; }
        
        public IEnumerable<CompensacaoAusenciaAlunoDto> Alunos { get; set; }
    }
}
