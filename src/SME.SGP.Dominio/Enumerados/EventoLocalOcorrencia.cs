using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum EventoLocalOcorrencia
    {
        [Display(Name = "Unidade Escolar")]
        UE = 1,

        [Display(Name = "Diretoria Municipal de Educação")]
        DRE = 2,

        [Display(Name = "Secretaria Municipal de Educação")]
        SME = 3,

        [Display(Name = "Secretaria Municipal de Educação / Unidade Escolar")]
        SMEUE = 4,

        [Display(Name = "Todos")]
        Todos = 5
    }
}