﻿using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoRelatorio
    {
        [Display(Name = "relatorios/alunos", ShortName = "RelatorioExemplo.pdf")]
        RelatorioExemplo = 1,

        [Display(Name = "relatorio/conselhoclassealuno", ShortName = "Ata de conselho de classe", Description = "Ata de conselho de classe")]
        ConselhoClasseAluno = 2,

        [Display(Name = "relatorio/conselhoclasseturma", ShortName = "Ata de conselho de classe", Description = "Ata de conselho de classe")]
        ConselhoClasseTurma = 3,

        [Display(Name = "relatorios/boletimescolar", ShortName = "BoletimEscolar", Description = "Boletim escolar")]
        Boletim = 4,

        [Display(Name = "relatorios/conselhoclasseatafinal", ShortName = "Ata de resultados finais", Description = "Ata de resultados finais")]
        ConselhoClasseAtaFinal = 5,

        [Display(Name = "relatorios/faltas-frequencia", ShortName = "RelatorioFaltasFrequencia", Description = "Relatório de faltas e frequência")]
        FaltasFrequencia = 6,

        [Display(Name = "relatorios/historicoescolarfundamental", ShortName = "HistoricoEscolar", Description = "Histórico Escolar")]
        HistoricoEscolarFundamental = 7,

        [Display(Name = "relatorios/fechamentopendencias", ShortName = "FechamentoPendencias", Description = "Relatório de Pendências do Fechamento")]
        FechamentoPendencias = 8,

        [Display(Name = "relatorios/parecerconclusivo", ShortName = "ParecerConclusivo", Description = "Relatório de Parecer Conclusivo")]
        ParecerConclusivo = 9,

        [Display(Name = "relatorios/recuperacaoparalela", ShortName = "RecuperacaoParalela", Description = "Relatório de Recuperação Paralela")]
        RecuperacaoParalela = 10,

        [Display(Name = "relatorios/notasconceitosfinais", ShortName = "NotasEConceitosFinais", Description = "Relatório de Notas e Conceitos Finais")]
        NotasEConceitosFinais = 11,

        [Display(Name = "relatorios/compensacaoausencia", ShortName = "CompensacaoAusencia", Description = "Relatório de Compensação de Ausência")]
        CompensacaoAusencia = 12,

        [Display(Name = "relatorios/impressaocalendario", ShortName = "Calendario", Description = "Relatório Impressão do Calendário")]
        Calendario = 13,

        [Display(Name = "relatorios/planoaula", ShortName = "Plano de Aula", Description = "Relatório Plano de Aula")]
        PlanoAula = 14,

        [Display(Name = "relatorios/resumopap", ShortName = "ResumoPAP", Description = "Relatório de acompanhamento PAP - Resumos")]
        ResumoPAP = 15,

        [Display(Name = "relatorios/sondagem/matematica-por-turma", ShortName = "Relatório de Sondagem (Matemática)", Description = "Relatório de Sondagem (Matemática)")]
        RelatorioMatetimaticaPorTurma = 16,

        [Display(Name = "relatorios/sondagem/matematica-consolidado", ShortName = "MatematicaConsolidado", Description = "Matematica Consolidado")]
        RelatorioMatetimaticaConsolidado = 17,

        [Display(Name = "relatorios/controle-grade", ShortName = "ControleGrade", Description = "Relatório Controle de Grade")]
        ControleGrade = 18,

        [Display(Name = "relatorios/notificacoes", ShortName = "Notificacoes", Description = "Relatório de Notificações")]
        Notificacoes = 19,        

        [Display(Name = "relatorios/usuarios", ShortName = "Usuarios", Description = "Relatório de Usuários")]
        Usuarios = 20,

        [Display(Name = "relatorios/atribuicoes-cj", ShortName = "Atribuição CJ", Description = "Relatório de Atribuições de CJ")]
        AtribuicaoCJ = 22,
        
        [Display(Name = "relatorios/graficopap", ShortName = "GraficoPAP", Description = "Relatório de acompanhamento PAP - Gráficos")]
        GraficoPAP = 23,

        [Display(Name = "relatorios/alteracao-notas", ShortName = "AlteracaoNotas", Description = "Relatório de alterações de notas")]
        AlteracaoNotasBimestre = 24,

        [Display(Name = "relatorios/ae/adesao", ShortName = "RelatorioDeAdesao", Description = "Relatório de Adesão do Escola Aqui")]
        AEAdesao = 25,

        [Display(Name = "relatorios/dados-leitura", ShortName = "DadosLeitura", Description = "Relatório de Leitura")]
        Leitura = 26,

        [Display(Name = "relatorios/planejamento-diario", ShortName = "Planejamento Diario", Description = "Relatório de controle de planejamento diário")]
        PlanejamentoDiario = 27,

        [Display(Name = "relatorios/devolutivas", ShortName = "Devolutivas", Description = "Relatório de Devolutiva")]
        Devolutivas = 28,

        [Display(Name = "relatorios/itinerancias", ShortName = "Itinerâncias", Description = "Relatório do Registro de Itinerância")]
        Itinerancias = 29,

        [Display(Name = "relatorios/registro-individual", ShortName = "RegistroIndividual", Description = "Relatório de Registro Individual")]
        RegistroIndividual = 30,

        [Display(Name = "relatorios/acompanhamento-aprendizagem", ShortName = "AcompanhamentoAprendizagem", Description = "Relatório do Acompanhamento da Aprendizagem")]
        AcompanhamentoAprendizagem = 31,

        [Display(Name = "relatorios/boletimescolardetalhado", ShortName = "BoletimEscolarDetalhado", Description = "Boletim escolar detalhado")]
        BoletimDetalhado = 32
    }
}
