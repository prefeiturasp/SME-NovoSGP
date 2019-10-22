using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarDto
    {
        [Range(1, 4, ErrorMessage = "É necessario informar o bimestre")]
        public int Bimestre { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "É necessario informar o fim do período escolar")]
        public DateTime PeriodoFim { get; set; }

        [Required(ErrorMessage = "É necessario informar o início do período escolar")]
        public DateTime PeriodoInicio { get; set; }
    }
}