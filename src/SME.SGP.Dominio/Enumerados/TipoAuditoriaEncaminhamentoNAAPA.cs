using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoAuditoriaEncaminhamentoNAAPA
    {
        [Display(Name = "Impressão")]
        Impressao = 1,

        [Display(Name = "Alteração")]
        Alteracao = 2,

        [Display(Name = "Exclusão")]
        Exclusao = 3
    }
}
