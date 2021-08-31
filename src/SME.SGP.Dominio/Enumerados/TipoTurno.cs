using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoTurno
    {
        [Display(Name = "Manhã")]
        manha = 1,
        [Display(Name = "Tarde")]
        Tarde = 2,
        [Display(Name = "Noturno")]
        Nortuno = 3,
    }
}
