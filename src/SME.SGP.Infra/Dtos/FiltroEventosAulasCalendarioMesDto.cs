using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroEventosAulasCalendarioMesDto : FiltroEventosAulasCalendarioDto
    {
        [Required(ErrorMessage = "O campo Mês é obrigatório")]
        public int Mes { get; set; }
    }
}