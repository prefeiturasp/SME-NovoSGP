using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum EntidadeStatus
    {
        [Display(Description = "Ativo")]
        Ativo = 1,

        [Display(Description = "Aguardando aprovação")]
        AguardandoAprovacao = 2
    }
}