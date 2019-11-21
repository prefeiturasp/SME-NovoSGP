using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPlanoAnualDisciplinaDto : FiltroPlanoAnualDto
    {
        [Required(ErrorMessage = "Disciplina deve ser informada para filtrar os objetivos")]
        public long DisciplinaId { get; set; }
    }
}
