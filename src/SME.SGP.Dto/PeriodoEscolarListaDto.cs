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
        [Required(ErrorMessage = "É necessario informar o tipo de calendario")]
        public int TipoCalendario { get; set; }
        
    }
}
