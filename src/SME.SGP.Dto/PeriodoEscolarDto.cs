using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class PeriodoEscolarDto
    {
        public long Codigo { get; set; }
        [Required(ErrorMessage = "É necessario informar o bimestre")]
        public int Bimestre { get; set; }
        [Required(ErrorMessage = "É necessario informar o incio do periodo escolar")]
        public DateTime PeriodoInicio { get; set; }
        [Required(ErrorMessage = "É necessario informar o fim do periodo escolar")]
        public DateTime PeriodoFim { get; set; }
    }
}
