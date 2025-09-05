using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum NivelAlfabetizacao
    {
        [Display(Name = "Pré-silábico", Description = "Sem relação entre fala e escrita")]
        PreSilabico = 1,

        [Display(Name = "Silábico sem valor", Description = "Troca sílabas por letras que não correspondem ao som")]
        SilabicoSemValor = 2,

        [Display(Name = "Silábico com valor", Description = "Representa sílabas com letras que correspondem ao som")]
        SilabicoComValor = 3,

        [Display(Name = "Silábico alfabético", Description = "Mistura de sílabas representadas por letra e sílabas completas")]
        SilabicoAlfabetico = 4,

        [Display(Name = "Alfabético", Description = "Escreve palavras completas, ainda que com erros ortográficos")]
        Alfabetico = 5
    }
}
