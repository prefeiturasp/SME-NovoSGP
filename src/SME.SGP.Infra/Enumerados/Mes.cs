using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Enumerados
{
    public enum Mes
    {
        [Display(Name = "Janeiro", ShortName = "Jan")]
        Janeiro = 1,

        [Display(Name = "Fevereiro", ShortName = "Fev")]
        Fevereiro = 2,

        [Display(Name = "março", ShortName = "Mar")]
        Março = 3,

        [Display(Name = "Abril", ShortName = "Abr")]
        Abril = 4,

        [Display(Name = "Maio", ShortName = "Maio")]
        Maio = 5,

        [Display(Name = "Junho", ShortName = "Jun")]
        Junho = 6,

        [Display(Name = "Julho", ShortName = "Jul")]
        Julho = 7,

        [Display(Name = "Agosto", ShortName = "Ago")]
        Agosto = 8,

        [Display(Name = "Setembro", ShortName = "Set")]
        Setembro = 9,

        [Display(Name = "Outubro", ShortName = "Out")]
        Outubro = 10,

        [Display(Name = "Novembro", ShortName = "Nov")]
        Novembro = 11,

        [Display(Name = "Dezembro", ShortName = "Dez")]
        Dezembro = 12,
    }
}
