using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class AulaPrevistaBimestreQuantidadeDto
    {
        [Required(ErrorMessage = "O bimestre deve ser informadas")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "A quantidade de aulas previstas deve ser informada")]
        [Range(0, 99, ErrorMessage = "A quantidade de aulas previstas deve ser maior que zero")]
        public int Quantidade { get; set; }
    }
}
