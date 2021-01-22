using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum WorkflowAprovacaoNivelStatus
    {
        [Display(Name = "Aguardando Aprovação")]
        AguardandoAprovacao = 1,

        [Display(Name = "Aprovado")]
        Aprovado = 2,

        [Display(Name = "Reprovado")]
        Reprovado = 3,

        [Display(Name = "Sem Status")]
        SemStatus = 4,

        [Display(Name = "Excluído")]
        Excluido = 5,
        
        [Display(Name = "Substituido")]
        Substituido = 6
    }
}