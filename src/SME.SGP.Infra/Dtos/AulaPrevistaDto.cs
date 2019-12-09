using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulaPrevistaDto
    {
        [Required(ErrorMessage = "A disciplina deve ser informada")]
        public string DisciplinaId { get; set; }

        [Required(ErrorMessage = "O tipo de calendário deve ser informado")]
        public long TipoCalendarioId { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public string TurmaId { get; set; }

        public IEnumerable<AulaPrevistaBimestreQuantidadeDto> BimestresQuantidade;
    }
}
