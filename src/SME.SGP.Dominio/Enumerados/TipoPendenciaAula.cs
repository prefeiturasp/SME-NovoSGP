using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoPendenciaAula
    {
        [Display(Name = "Frequência")]
        Frequencia = 1,

        [Display(Name = "Plano de Aula")]
        PlanoAula = 2,

        [Display(Name = "Diario de Bordo")]
        DiarioBordo = 3,

        [Display(Name = "Avaliação")]
        Avaliacao = 4,
    }
}
