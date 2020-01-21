using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class CompensacaoAusenciaCopiaDto
    {
        [Required(ErrorMessage = "Deve ser informada a compensação de origem da copia!")]
        public long CompensacaoOrigemId { get; set; }

        public string TurmaId { get; set; }

        public int Bimestre { get; set; }
    }
}
