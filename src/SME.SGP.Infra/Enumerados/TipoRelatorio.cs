using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public enum TipoRelatorio
    {
        [Display(Name = "relatorios/alunos")]
        Games = 1,

        [Display(Name = "relatorios/boletim")]
        Boletim = 4
    }
}
