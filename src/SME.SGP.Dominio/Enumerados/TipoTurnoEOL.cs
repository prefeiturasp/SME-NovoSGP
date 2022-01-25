using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoTurnoEOL
    {
        [Display(Name = "Manhã")]
        Manha = 1,
        [Display(Name = "Intermediário")]
        Intermediario = 2,
        [Display(Name = "Tarde")]
        Tarde = 3,
        [Display(Name = "Vespertino")]
        Vespertino = 4,
        [Display(Name = "Noite")]
        Noite = 5,
        [Display(Name = "Integral")]
        Integral = 6,
    }
}
