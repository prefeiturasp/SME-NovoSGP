using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoRelatorio
    {
        [Display(Name = "relatorios/alunos", ShortName = "RelatorioExemplo.pdf")]
        RelatorioExemplo = 1,

        [Display(Name = "relatorio/conselhoclassealuno", ShortName = "RelatorioConselhoClasse", Description = "Relatório de conselho de classe")]
        ConselhoClasseAluno = 2,

        [Display(Name = "relatorio/conselhoclasseturma", ShortName = "RelatorioConselhoTurma", Description = "Relatório de conselho de classe")]
        ConselhoClasseTurma = 3,

        [Display(Name = "relatorios/boletimescolar", ShortName = "BoletimEscolar", Description = "Boletim escolar")]
        Boletim = 4,

        [Display(Name = "relatorios/conselhoclasseatafinal", ShortName = "RelatorioConselhoClasseAtaFinal", Description = "Conselho Classe Ata Final")]
        ConselhoClasseAtaFinal = 5,

        [Display(Name = "relatorios/faltas-frequencia", ShortName = "RelatorioFaltasFrequencia", Description = "Relatório de faltas e frequência")]
        FaltasFrequencia = 6,
        [Display(Name = "relatorios/historicoescolarfundamental", ShortName = "HistoricoEscolar", Description = "Histórico Escolar")]
        HistoricoEscolarFundamental = 7,

        [Display(Name = "relatorios/fechamentopendencias", ShortName = "FechamentoPendencias", Description = "Relatório de Pendências do Fechamento")]
        FechamentoPendencias = 8,

    }
}
