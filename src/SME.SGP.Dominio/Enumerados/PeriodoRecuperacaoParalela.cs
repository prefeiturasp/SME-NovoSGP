using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum PeriodoRecuperacaoParalela
    {
        [Display(Name = "Encaminhamento")]
        Encaminhamento = 1,

        [Display(Name = "Acompanhamento 1º Semestre")]
        AcompanhamentoPrimeiroSemestre = 2,

        [Display(Name = "Acompanhamento 2º Semestre")]
        AcompanhamentoSegundoSemestre = 3,
    }
}