using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoValor
    {
        [Display(Name = "Numerico")]
        Numerico = 1,
        [Display(Name = "Textual")]
        Textual = 2
    }
}
