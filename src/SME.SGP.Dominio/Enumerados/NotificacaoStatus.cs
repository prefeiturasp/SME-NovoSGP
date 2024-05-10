using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum NotificacaoStatus
    {
        [Display(Name = "Não lida")]
        Pendente = 1,

        [Display(Name = "Lida")]
        Lida = 2,

        [Display(Name = "Aprovada")]
        Aceita = 3,

        [Display(Name = "Recusada")]
        Reprovada = 4
    }
}