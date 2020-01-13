using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum EntidadeStatus
    {
        [Display(Description = "Aprovado")]
        Aprovado = 1,

        [Display(Description = "Aguardando aprovação")]
        AguardandoAprovacao = 2,

        [Display(Description = "Recusado")]
        Recusado = 3
    }
}