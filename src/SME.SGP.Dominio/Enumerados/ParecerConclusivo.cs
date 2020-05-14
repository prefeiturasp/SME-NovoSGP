using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum ParecerConclusivo
    {
        [Display(Name = "Aprovado")]
        Aprovado = '1',

        [Display(Name = "Reprovado")]
        Reprovado = '2',

        [Display(Name = "Reprovado por Nota")]
        ReprovadoNota = '3',
    }
}