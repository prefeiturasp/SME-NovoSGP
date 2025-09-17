using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

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

        [Display(Name = "relatorios/conselhoclasseatafinal", ShortName = "Ata final de resultados", Description = "Relatório Ata final de resultados")]
        ConselhoClasseAtaFinal = 5,

        [Display(Name = "relatorios/frequencia", ShortName = "RelatorioFrequencia", Description = "Relatório de frequência")]
        Frequencia = 6,

        [Display(Name = "relatorios/historicoescolarfundamental", ShortName = "HistoricoEscolar", Description = "Histórico Escolar")]
        HistoricoEscolarFundamental = 7,

        [Display(Name = "relatorios/pendencias", ShortName = "Pendencias", Description = "Relatório de Pendências")]
        Pendencias = 8,

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
        BoletimDetalhado = 32,

        [Display(Name = "relatorios/acompanhamento-fechamento", ShortName = "AcompanhamentoFechamento", Description = "Relatório do Acompanhamento de Fechamento")]
        AcompanhamentoFechamento = 33,

        [Display(Name = "relatorios/conselhoclasseatabimestral", ShortName = "AtaBimestral", Description = "Relatório de Ata Bimestral")]
        AtaBimestral = 34,

        [Display(Name = "relatorios/boletimescolardetalhadoescolaaqui", ShortName = "Boletim", Description = "Boletim escolar detalhado")]
        BoletimDetalhadoApp = 35,

        [Display(Name = "relatorios/acompanhamento-registrospedagogicos", ShortName = "AcompanhamentoRegistrosPedagogicos", Description = "Relatório do Acompanhamento de Registros Pedagógicos")]
        AcompanhamentoRegistrosPedagogicos = 36,

        [Display(Name = "relatorios/acompanhamento-frequencia", ShortName = "AcompanhamentoFrequencia", Description = "Relatório de Frequência Individual")]
        AcompanhamentoFrequencia = 37,

        [Display(Name = "relatorios/ocorrencias", ShortName = "RelatorioOcorrencia", Description = "Relatório de ocorrências")]
        RelatorioOcorrencias = 38,

        [Display(Name = "relatorios/acompanhamento-aprendizagem-escolaaqui", ShortName = "RAA", Description = "Relatório do Acompanhamento da Aprendizagem Escola Aqui")]
        RaaEscolaAqui = 39,

        [Display(Name = "relatorios/frequencia-global", ShortName = "RelatorioFrequenciaMensal", Description = "Relatório de frequência mensal")]
        FrequenciaMensal = 40,
        
        [Display(Name = "relatorios/planoaee", ShortName = "PlanoAEE", Description = "Plano AEE")]
        PlanoAee = 41,
        
        [Display(Name = "relatorios/planosaee", ShortName = "PlanoAEE", Description = "Plano AEE")]
        RelatorioPlanosAee = 42,

        [Display(Name = "relatorios/encaminhamentosaee", ShortName = "EncaminhamentoAEE", Description = "Encaminhamento AEE")]
        RelatorioEncaminhamentosAee = 43,
        
        [Display(Name = "relatorios/encaminhamentoaeedetalhado", ShortName = "EncaminhamentoAEE", Description = "Encaminhamento AEE")]
        RelatorioEncaminhamentoAeeDetalhado = 44,

        [Display(Name = "relatorios/encaminhamentosnaapa", ShortName = "EncaminhamentoNAAPA", Description = "Relatório de Encaminhamento NAAPA")]
        RelatorioEncaminhamentosNAAPA = 45,
        
        [Display(Name = "relatorios/historicoescolarfundamental", ShortName = "HistoricoEscolar", Description = "Histórico Escolar")]
        HistoricoEscolarFundamentalRazor = 46,
        
        [Display(Name = "relatorios/encaminhamentonaapadetalhado", ShortName = "EncaminhamentoNAAPA", Description = "Relatório do Encaminhamento NAAPA")]
        RelatorioEncaminhamentoNaapaDetalhado = 47,

        [Display(Name = "relatorios/analitico-sondagem", ShortName = "AnaliticoSondagem", Description = "Relatório analítico da Sondagem")]
        RelatorioAnaliticoSondagem = 48,

        [Display(Name = "relatorios/listagem-itinerancias", ShortName = "Itinerâncias", Description = "Relatório de Registro de Itinerância")]
        ListagemItinerancias = 49,

        [Display(Name = "relatorios/controle-frequencia-mensal", ShortName = "Frequência", Description = "Relatório de Controle de frequência mensal")]
        RelatorioControleFrequenciaMensal = 50,

        [Display(Name = "relatorios/historicoescolareja", ShortName = "HistoricoEscolar", Description = "Histórico Escolar")]
        HistoricoEscolarEJARazor = 51,

        [Display(Name = "relatorios/listagem-ocorrencias", ShortName = "ListagemOcorrencia", Description = "Relatório de Ocorrências")]
        ListagemOcorrencias = 52,
            
        [Display(Name = "relatorios/plano-anual", ShortName = "Plano Anual", Description = "Relatório Plano Anual")]
        RelatorioPlanoAnual = 53,

        [Display(Name = "relatorios/mapeamentosestudantes", ShortName = "MapeamentosEstudantes", Description = "Relatório de Mapeamento de estudantes")]
        RelatorioMapeamentosEstudantes = 54,

        [Display(Name = "relatorios/buscasativas", ShortName = "BuscasAtivas", Description = "Relatório de Busca ativa")]
        RelatorioBuscasAtivas = 55,

        [Display(Name = "relatorios/produtividade-frequencia", ShortName = "RelatorioProdutividadeFrequencia", Description = "Relatório de Produtividade de Frequência")]
        ProdutividadeFrequencia = 56,

        [Display(Name = "relatorios/frequencia-global-todos", ShortName = "RelatorioFrequenciaMensalTodosDreUe", Description = "Relatório de frequência mensal filtro todos dre ou ue")]
        FrequenciaMensalTodosDreUe = 57,
    }
}
