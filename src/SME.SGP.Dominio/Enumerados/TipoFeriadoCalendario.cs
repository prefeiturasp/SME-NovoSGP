using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoFeriadoCalendario
    {
        [Display(Name = "Fixo")]
        Fixo = 1,

        [Display(Name = "Móvel")]
        Movel = 2
    }
}