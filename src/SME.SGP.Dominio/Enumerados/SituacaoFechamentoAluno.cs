using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum SituacaoFechamentoAluno
    {
        [Display(Name = "Sem Registros")]
        SemRegistros = 1,

        [Display(Name = "Parcial")]
        Parcial = 2,

        [Display(Name = "Completo")]
        Completo = 3,

    }
}