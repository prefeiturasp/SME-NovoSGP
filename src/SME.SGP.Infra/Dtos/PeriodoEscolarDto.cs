using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class PeriodoEscolarDto
    {
        public long Id { get; set; }
        [Range(1, 4, ErrorMessage = "É necessario informar o bimestre")]
        public int Bimestre { get; set; }
        [Required(ErrorMessage = "É necessario informar o início do período escolar")]
        public DateTime PeriodoInicio { get; set; }
        [Required(ErrorMessage = "É necessario informar o fim do período escolar")]
        public DateTime PeriodoFim { get; set; }
    }
}
