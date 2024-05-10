using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum AbrangenciaFeriadoCalendario
    {
        [Display(Name = "Nacional")]
        Nacional = 1,

        [Display(Name = "Estadual")]
        Estadual = 2,

        [Display(Name = "Municipal")]
        Municipal = 3,
    }
}