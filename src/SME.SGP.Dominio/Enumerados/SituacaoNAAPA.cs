using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoNAAPA
    {
        [Display(Name = "Rascunho")]
        Rascunho = 1,
        [Display(Name = "Aguardando atendimento")]
        AguardandoAtendimento = 2
    }
}
