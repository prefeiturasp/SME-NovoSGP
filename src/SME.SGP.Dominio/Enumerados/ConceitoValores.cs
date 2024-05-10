using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum ConceitoValores
    {
        [Display(Name = "Plenamente Satisfatório")]
        P = 1,

        [Display(Name = "Satisfatório")]
        S = 2,

        [Display(Name = "Não Satisfatório")]
        NS = 3,
    }
}
