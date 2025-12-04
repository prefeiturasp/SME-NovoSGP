using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoHistoricoAlteracoesAtendimentoNAAPA
    {
        [Display(Name = "Impressão")]
        Impressao = 1,

        [Display(Name = "Inserido")]
        Inserido = 2,

        [Display(Name = "Alterado")]
        Alteracao = 3,

        [Display(Name = "Excluído")]
        Exclusao = 4
    }
}
