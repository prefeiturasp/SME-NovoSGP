using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroEventosAulasCalendarioDto
    {
        [Required(ErrorMessage = "O campo DreId é obrigatório")]
        public string DreId { get; set; }

        [Required(ErrorMessage = "O campo Evento SME é obrigatório")]
        public bool EhEventoSme { get; set; }

        [Required(ErrorMessage = "O campo TipoCalendarioId é obrigatório")]
        public long TipoCalendarioId { get; set; }

        public bool TodasTurmas { get; set; }

        public bool TurmaHistorico { get; set; }
        public string TurmaId { get; set; }

        [Required(ErrorMessage = "O campo UeId é obrigatório")]
        public string UeId { get; set; }
    }
}