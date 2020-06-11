using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoRelatorio
    {
        [Display(Name = "relatorios/alunos")]
        RelatorioExemplo = 1,

        [Display(Name = "relatorios/boletim")]
        Boletim = 4
    }
}
