using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroEventosAulasCalendarioDiaDto : FiltroEventosAulasCalendarioDto
    {
        [Required(ErrorMessage = "O campo Data é obrigatório")]
        public DateTime Data { get; set; }
    }
}