using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoRelatorio
    {
        [Display(Name = "relatorios/alunos", ShortName = "RelatorioExemplo.pdf")]
        RelatorioExemplo = 1,

        [Display(Name = "relatorios/parecerconclusivo", ShortName = "RelatorioParecerConclusivo.pdf", Description = "Relatório de Pareceres Conclusivos da Turma")]
        ParecerConclusivo = 5
    }
}
