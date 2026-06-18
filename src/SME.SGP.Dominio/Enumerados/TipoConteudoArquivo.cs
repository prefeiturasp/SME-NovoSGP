using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoConteudoArquivo
    {
        [Display(Name = "")]
        Indefinido = 0,

        [Display(Name = "application/pdf")]
        PDF = 1,
    }
}
