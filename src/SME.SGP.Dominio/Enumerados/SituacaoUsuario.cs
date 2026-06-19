using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum SituacaoUsuario
    {
        [Display(Name = "Ativo")]
        Ativo = 1,

        [Display(Name = "Bloqueado")]
        Bloqueado,

        [Display(Name = "Excluido")]
        Excluido,

        [Display(Name = "Padrão Sistema")]
        PadraoSistema,

        [Display(Name = "Senha Expirada")]
        SenhaExpirada
    }
}
