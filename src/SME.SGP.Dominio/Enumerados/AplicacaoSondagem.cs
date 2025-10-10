using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum AplicacaoSondagem
    {
        [Display(Name = "SGP")]
        SGP = 1,

        [Display(Name = "Sondagem Aplicação")]
        SondagemAplicacao = 2,

        [Display(Name = "Sondagem Digitação")]
        SondagemDigitacao = 3
    }
}