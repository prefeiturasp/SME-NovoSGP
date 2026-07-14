using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum SinteseEnum
    {
        [Display(Name = "Frequente")]
        Frequente = 1,

        [Display(Name = "Não frequente")]
        NaoFrequente = 2
    }
}
