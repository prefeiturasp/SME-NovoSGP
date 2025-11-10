using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoPap
    {
        [Display(Name = "Pap Colaborativo")]
        PapColaborativo = 1,

        [Display(Name = "Recuperação de Aprendizagens")]
        RecuperacaoAprendizagens = 2,

        [Display(Name = "Pap 2º Ano")]
        Pap2Ano = 3
    }
}