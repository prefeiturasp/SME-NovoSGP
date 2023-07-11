using System;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RelatorioPAP.Base
{
    public class RelatorioPAPTesteBase : TesteBaseComuns
    {
        public RelatorioPAPTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase(bool criarPeriodoEscolar = true)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilPAP());

            await CriarUsuarios();

            await CriarTurmaTipoCalendario();

            if (criarPeriodoEscolar)
                await CriarPeriodoEscolar(criarPeriodoEscolar);
            
            await CriarConfiguracaoRelatorio();

            await CriarPeriodoRelatorio();
            
            await CriarPeriodoEscolarRelatorio();

            await CriarQuestionario();
            
            await CriarSecaoRelatorioPeriodico();

            await CriarSecaoConfRelatorioPeriodico();

            await CriarQuestoes();

            await CriarOpcaoResposta();
        }

        private async Task CriarOpcaoResposta()
        {
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Nome = ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Nome = ConstantesTestePAP.OPCAO_RESPOSTA_ESCRITA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Nome = ConstantesTestePAP.OPCAO_RESPOSTA_CALCULOS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Nome = ConstantesTestePAP.OPCAO_RESPOSTA_INTERPRETACAO_TEXTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoes()
        {
            await InserirNaBase(new Questao()
            {
                Id = ConstantesTestePAP.QUESTAO_ID_1,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                Nome = string.Empty,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Obrigatorio = false,
                Tipo = TipoQuestao.InformacoesFrequenciaTurmaPAP,
                Dimensao = ConstantesTestePAP.DIMENSAO_12,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_INFO_FREQ_TURMA_PAP,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Questao()
            {
                Id = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Nome = ConstantesTestePAP.DIFICULDADES_APRESENTADAS,
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                Dimensao = ConstantesTestePAP.DIMENSAO_6,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_DIFIC_APRESENTADAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Questao()
            {
                Id = ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                Ordem = ConstantesTestePAP.ORDEM_2,
                Nome = ConstantesTestePAP.DIFICULDADES_APRESENTADAS,
                Obrigatorio = false,
                Tipo = TipoQuestao.EditorTexto,
                Dimensao = ConstantesTestePAP.DIMENSAO_6,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_OBS_DIFIC_APRESENTADAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Questao()
            {
                Id = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Nome = ConstantesTestePAP.AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE,
                Obrigatorio = true,
                Tipo = TipoQuestao.EditorTexto,
                Dimensao = ConstantesTestePAP.DIMENSAO_6,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_AVANC_APREND_BIMES,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Questao()
            {
                Id = ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Nome = ConstantesTestePAP.OBSERVACOES,
                Obrigatorio = false,
                Tipo = TipoQuestao.EditorTexto,
                Dimensao = ConstantesTestePAP.DIMENSAO_6,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_OBS_OBS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarSecaoConfRelatorioPeriodico()
        {
            var campos = new[] { ConstantesTestePAP.CAMPO_SECAO_RELATORIO_PERIODICO_PAP_ID, 
                                       ConstantesTestePAP.CAMPO_CONFIGURACAO_RELATORIO_PAP_ID, 
                                       ConstantesTestePAP.CAMPO_CRIADO_EM, 
                                       ConstantesTestePAP.CAMPO_CRIADO_POR, 
                                       ConstantesTestePAP.CAMPO_CRIADO_RF };

            var valores = new[]
            {
                ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_FREQUENCIA_NA_TURMA_PAP_ID_1.ToString(),
                ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_1.ToString(),
                $"'{DateTimeExtension.HorarioBrasilia().ToString("yyyy-MM-dd HH:mm:ss")}'",
                $"'{SISTEMA_NOME}'",
                $"'{SISTEMA_CODIGO_RF}'"
            };
            await InserirNaBase(ConstantesTestePAP.TABELA_SECAO_CONFIG_RELATORIO_PERIODICO_PAP, campos,valores);
            
            valores = new[]
            {
                ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2.ToString(),
                ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_1.ToString(),
                $"'{DateTimeExtension.HorarioBrasilia().ToString("yyyy-MM-dd HH:mm:ss")}'",
                $"'{SISTEMA_NOME}'",
                $"'{SISTEMA_CODIGO_RF}'"
            };
            await InserirNaBase(ConstantesTestePAP.TABELA_SECAO_CONFIG_RELATORIO_PERIODICO_PAP, campos,valores);
            
            valores = new[]
            {
                ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3.ToString(),
                ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_1.ToString(),
                $"'{DateTimeExtension.HorarioBrasilia().ToString("yyyy-MM-dd HH:mm:ss")}'",
                $"'{SISTEMA_NOME}'",
                $"'{SISTEMA_CODIGO_RF}'"
            };
            await InserirNaBase(ConstantesTestePAP.TABELA_SECAO_CONFIG_RELATORIO_PERIODICO_PAP, campos,valores);
            
            valores = new[]
            {
                ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4.ToString(),
                ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_1.ToString(),
                $"'{DateTimeExtension.HorarioBrasilia().ToString("yyyy-MM-dd HH:mm:ss")}'",
                $"'{SISTEMA_NOME}'",
                $"'{SISTEMA_CODIGO_RF}'"
            };
            await InserirNaBase(ConstantesTestePAP.TABELA_SECAO_CONFIG_RELATORIO_PERIODICO_PAP, campos,valores);
        }

        private async Task CriarSecaoRelatorioPeriodico()
        {
            await InserirNaBase(new SecaoRelatorioPeriodicoPAP()
            {
                Id = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_FREQUENCIA_NA_TURMA_PAP_ID_1,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_SECAO_FREQUENCIA,
                Nome = ConstantesTestePAP.FREQUENCIA_NA_TURMA_PAP,
                Ordem = ConstantesTestePAP.ORDEM_1,
                Etapa = ConstantesTestePAP.ETAPA_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new SecaoRelatorioPeriodicoPAP()
            {
                Id = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_SECAO_DIFIC_APRES,
                Nome = ConstantesTestePAP.DIFICULDADES_APRESENTADAS,
                Ordem = ConstantesTestePAP.ORDEM_2,
                Etapa = ConstantesTestePAP.ETAPA_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new SecaoRelatorioPeriodicoPAP()
            {
                Id = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_SECAO_AVANC_APREND_BIMES,
                Nome = ConstantesTestePAP.AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE,
                Ordem = ConstantesTestePAP.ORDEM_3,
                Etapa = ConstantesTestePAP.ETAPA_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new SecaoRelatorioPeriodicoPAP()
            {
                Id = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                QuestionarioId = ConstantesTestePAP.QUESTIONARIO_ID_1,
                NomeComponente = ConstantesTestePAP.NOME_COMPONENTE_SECAO_OBS,
                Nome = ConstantesTestePAP.OBSERVACOES,
                Ordem = ConstantesTestePAP.ORDEM_4,
                Etapa = ConstantesTestePAP.ETAPA_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestionario()
        {
            await InserirNaBase(new Questionario()
            {
                Id = ConstantesTestePAP.QUESTIONARIO_ID_1,
                Nome = ConstantesTestePAP.QUESTIONARIO_NOME_1,
                Tipo = TipoQuestionario.RelatorioPAP,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarPeriodoEscolarRelatorio()
        {
            foreach (var periodoEscolar in ObterTodos<PeriodoEscolar>())
            {
                await InserirNaBase(new PeriodoEscolarRelatorioPAP()
                {
                    PeriodoRelatorioId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1,
                    PeriodoEscolarId = periodoEscolar.Id,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        private async Task CriarPeriodoRelatorio()
        {
            await InserirNaBase(new PeriodoRelatorioPAP()
            {
                ConfiguracaoId = ConstantesTestePAP.CONFIGURACAO_RELATORIO_PAP_ID_1,
                Periodo = ConstantesTestePAP.PERIODO_SEGUNDO_SEMESTRE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarConfiguracaoRelatorio()
        {
            await InserirNaBase(new ConfiguracaoRelatorioPAP()
            {
                InicioVigencia = DATA_03_01_INICIO_BIMESTRE_1,
                FimVigencia = DATA_22_12_FIM_BIMESTRE_4,
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_SEMANAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarTurmaTipoCalendario()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio, false);
            await CriarTurma(Modalidade.Fundamental, "7", false, TipoTurma.Programa);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }
    }
}
