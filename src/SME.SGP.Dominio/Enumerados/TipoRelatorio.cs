using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoRelatorio
    {
        [Display(Name = "relatorios/alunos", ShortName = "RelatorioExemplo.pdf")]
        RelatorioExemplo = 1,

        [Display(Name = "relatorio/conselhoclassealuno" , ShortName = "RelatorioConselhoClasse.pdf")]
        ConselhoClasseAluno = 2,

        [Display(Name = "relatorio/conselhoclasseturma", ShortName = "RelatorioConselhoTurma.pdf")]
        ConselhoClasseTurma = 3
    }
}
