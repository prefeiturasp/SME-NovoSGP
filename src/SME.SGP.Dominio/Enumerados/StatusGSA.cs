using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum StatusGSA
    {
        [Display(Name = "Não Especificado")]
        NaoEspecificado = 0,
        [Display(Name = "Publicado")]
        Publicado = 1,
        [Display(Name = "Rascunho")]
        Rascunho = 2,
        [Display(Name = "Deletado")]
        Deletado = 3
    }
}
