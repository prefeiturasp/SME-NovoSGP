using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoAula
    {
        [Display(Name = "Normal")]
        Normal = 1,

        [Display(Name = "Reposição")]
        Reposicao =2
    }
}
