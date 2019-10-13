using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class PeriodoEscolarListaDto
    {
        [Required(ErrorMessage = "Nenhum periodo foi informado")]
        public List<PeriodoEscolarDto> Periodos { get; set; }
        public bool Eja { get; set; }
    }
}
