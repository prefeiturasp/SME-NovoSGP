using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum TipoPeriodoDashboardFrequencia
    {
        [Display(Name = "Diário")]
        Diario = 1,
        [Display(Name = "Semanal")]
        Semanal = 2,
        [Display(Name = "Mensal")]
        Mensal = 3,
    }
}
