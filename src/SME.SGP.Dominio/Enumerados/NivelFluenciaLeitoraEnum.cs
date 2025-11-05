using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum NivelFluenciaLeitoraEnum
    {
        [Display(Name = "Pré-leitor 1")]
        Fluencia1 = 1,

        [Display(Name = "Pré-leitor 2")]
        Fluencia2 = 2,

        [Display(Name = "Pré-leitor 3")]
        Fluencia3 = 3,

        [Display(Name = "Pré-leitor 4")]
        Fluencia4 = 4,

        [Display(Name = "Leitor iniciante")]
        Fluencia5 = 5,

        [Display(Name = "Leitor fluente")]
        Fluencia6 = 6,
    }
}
