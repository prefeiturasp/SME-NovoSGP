using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum NotificacaoCategoria
    {
        [Display(Name = "Alerta")]
        Alerta = 1,

        [Display(Name = "Ação")]
        Workflow_Aprovacao = 2,

        [Display(Name = "Aviso")]
        Aviso = 3
    }
}