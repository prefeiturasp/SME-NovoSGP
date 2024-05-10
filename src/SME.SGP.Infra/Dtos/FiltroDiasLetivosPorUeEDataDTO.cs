using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroDiasLetivosPorUeEDataDTO
    {
        [Required(ErrorMessage = "O código da escola é obrigatório")]
        public string UeCodigo { get; set; }

        [Required(ErrorMessage = "O Tipo do turno é obrigatório")]
        public int TipoTurno { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data fim é obrigatória")]
        public DateTime DataFim { get; set; }
    }
}
