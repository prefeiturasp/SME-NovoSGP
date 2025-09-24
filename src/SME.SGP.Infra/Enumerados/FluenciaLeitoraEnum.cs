using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Enumerados
{
    public enum FluenciaLeitoraEnum
    {
        [Display(Name = "Pré-leitor 1"), Description("Não leu")]
        Fluencia1 = 1,

        [Display(Name = "Pré-leitor 2"), Description("Soletrou")]
        Fluencia2 = 2,

        [Display(Name = "Pré-leitor 3"), Description("Silabou")]
        Fluencia3 = 3,

        [Display(Name = "Pré-leitor 4"), Description("Leu até 10 palavras")]
        Fluencia4 = 4,

        [Display(Name = "Leitor iniciante")]
        Fluencia5 = 5,

        [Display(Name = "Leitor fluente")]
        Fluencia6 = 6,
    }
}
