using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroDiasLetivosDTO
    {
        public string DreId { get; set; }

        [Required(ErrorMessage = "O Tipo de Calendário é obrigatório")]
        [Range(1, double.MaxValue, ErrorMessage = "O Tipo de Calendário é obrigatório")]
        public long TipoCalendarioId { get; set; }

        public string UeId { get; set; }
    }
}