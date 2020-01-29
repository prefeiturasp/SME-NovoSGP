using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FechamentoTurmaDisciplinaDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "A turma é obrigatória!")]
        public string TurmaId { get; set; }

        [Required(ErrorMessage = "O bimestre é obrigatório")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "A disciplina é obrigatória!")]
        public string DisciplinaId { get; set; }
    }
}
