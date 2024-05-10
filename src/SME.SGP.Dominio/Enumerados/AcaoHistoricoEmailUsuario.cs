using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum AcaoHistoricoEmailUsuario
    {
        [Display(Name = "Reiniciar Senha")]
        ReiniciarSenha = 1,

        [Display(Name = "Alterar Email")]
        AlterarEmail = 2
    }
}
