using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoAEE
    {
        [Display(Name = "Rascunho")]
        Rascunho = 1,
        [Display(Name = "Encaminhado")]
        Encaminhado = 2,
        [Display(Name = "Finalizado")]
        Finalizado = 3,
        [Display(Name = "Encerrado")]
        Encerrado = 4,
    }
}
