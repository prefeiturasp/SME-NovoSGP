using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoResponsavel
    {
        [Display(Name = "Filiação 1")]
        Filicacao1 = 1,
        [Display(Name = "Filiação 2")]
        Filiacao2 = 2,
        [Display(Name = "Responsável Legal")]
        ResponsavelLegal = 3,
        [Display(Name = "Próprio estudante")]
        ProprioEstudante = 4
    }
}
