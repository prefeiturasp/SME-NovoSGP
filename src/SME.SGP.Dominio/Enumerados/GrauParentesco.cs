using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{    
    public enum GrauParentesco
    {
        [Display(Name = "Avós")]
        Avos = 1,
        [Display(Name = "Gerente de Serviços")]
        GerenteServicos = 2,
        [Display(Name = "Irmãos")]
        Irmaos = 3,
        [Display(Name = "Madrasta / Padrasto")]
        MadrastaPadrasto = 4,
        [Display(Name = "Madrinha / Padrinho")]
        MadrinhaPadrinho = 5,
        [Display(Name = "Mãe")]
        Mae = 6,
        [Display(Name = "Pai")]
        Pai = 7,
        [Display(Name = "Primos")]
        Primos = 8,
        [Display(Name = "Tios")]
        Tios = 9,
        [Display(Name = "Vizinhos")]
        Vizinhos = 10
    }
}
