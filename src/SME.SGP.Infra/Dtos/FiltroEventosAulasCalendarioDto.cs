using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroEventosAulasCalendarioDto
    {
        [Required(ErrorMessage = "O campo DreId é obrigatório")]
        public string DreId { get; set; }

        [Required(ErrorMessage = "O campo TipoCalendarioId é obrigatório")]
        public long TipoCalendarioId { get; set; }

        [Required(ErrorMessage = "O campo TurmaId é obrigatório")]
        public string TurmaId { get; set; }

        [Required(ErrorMessage = "O campo UeId é obrigatório")]
        public string UeId { get; set; }
    }
}