using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoFrequencia
    {
        [Display(Name = "Compareceu")]
        C = 1,
        [Display(Name = "Faltou")]
        F = 2,
        [Display(Name = "Remoto")]
        R = 3,
    }
}
