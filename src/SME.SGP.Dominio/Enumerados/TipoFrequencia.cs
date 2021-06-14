using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoFrequencia
    {
        [Display(Name = "Compareceu", ShortName = "C")]
        C = 1,
        [Display(Name = "Faltou", ShortName = "F")]
        F = 2,
        [Display(Name = "Remoto", ShortName = "R")]
        R = 3,
    }
}
