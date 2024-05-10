using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum CategoriaAtividadeAvaliativa
    {
        [Display(Name = "Normal")]
        Normal = 1,

        [Display(Name = "Interdisciplinar")]
        Interdisciplinar = 2
    }
}