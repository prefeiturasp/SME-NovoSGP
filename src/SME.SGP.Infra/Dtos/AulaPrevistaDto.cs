using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulaPrevistaDto
    {
        [Required(ErrorMessage = "O bimestre deve ser informadas")]
        public int Bimestre  { get; set; }

        [Required(ErrorMessage = "A disciplina deve ser informada")]
        public string DisciplinaId { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "A quantidade de aulas previstas deve ser informada")]
        [Range(1, 99, ErrorMessage = "A quantidade de aulas previstas deve ser maior que zero")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "O tipo de calendário deve ser informado")]
        public long TipoCalendarioId { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public string TurmaId { get; set; }
    }
}
