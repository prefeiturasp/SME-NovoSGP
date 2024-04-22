using Nest;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base
{
    public class MapeamentoBase : TesteBaseComuns
    {
        protected const long SECAO_MAPEAMENTO_ESTUDANTE_ID_1 = 1;
        protected const string NOME_COMPONENTE_SECAO_1_MAPEAMENTO_ESTUDANTE = "SECAO_1_MAPEAMENTO_ESTUDANTE";

        protected List<OpcaoResposta> OpcoesResposta;
        protected List<Questao> Questoes;
        protected Dictionary<string, long> IdsQuestoesPorNomeComponente = new();

        public MapeamentoBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected virtual async Task CriarDadosBase()
        {
            ExecutarScripts(new List<ScriptCarga> { ScriptCarga.CARGA_QUESTIONARIO_MAPEAMENTO_ESTUDANTE });

            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await CriarTipoCalendario(Dominio.ModalidadeTipoCalendario.FundamentalMedio, false);

            await CriarTurma(Dominio.Modalidade.Fundamental, ANO_4, false, tipoTurno: 2);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
        }

        protected void CarregarDadosBase()
        {
            OpcoesResposta = ObterTodos<OpcaoResposta>();
            Questoes = ObterTodos<Questao>();
            IdsQuestoesPorNomeComponente.Clear();
        }

        protected async Task GerarDadosMapeamentosEstudantes_1()
        {
            var mapeamentoId = await CriarMapeamentoEstudante();
            var mapeamentoSecaoId = await CriarMapeamentoEstudanteSecao(mapeamentoId);
            await CriarQuestoesMapeamentoEstudante(mapeamentoSecaoId);
            await CriarRespostasMapeamentoEstudante();
        }

        private async Task CriarRespostasMapeamentoEstudante()
        {
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.PARECER_CONCLUSIVO_ANO_ANTERIOR)).Value,
                Texto = "{'index': '1', 'value': 'Promovido'}",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.TURMA_ANO_ANTERIOR)).Value,
                Texto = "EF-4B",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)).Value,
                Texto = "ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.CLASSIFICADO)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.CLASSIFICADO)).Id
                                              && q.Nome == "Sim").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.RECLASSIFICADO)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.RECLASSIFICADO)).Id
                                              && q.Nome == "Sim").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.MIGRANTE)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.MIGRANTE)).Id
                                              && q.Nome == "Sim").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.ACOMPANHADO_SRM_CEFAI)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.ACOMPANHADO_SRM_CEFAI)).Id
                                              && q.Nome == "Sim").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.POSSUI_PLANO_AEE)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.POSSUI_PLANO_AEE)).Id
                                              && q.Nome == "Sim").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.ACOMPANHADO_NAAPA)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.ACOMPANHADO_NAAPA)).Id
                                              && q.Nome == "Sim").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.ACOES_REDE_APOIO)).Value,
                Texto = "AÇÕES DA REDE DE APOIO",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.ACOES_RECUPERACAO_CONTINUA)).Value,
                Texto = "AÇÕES DE RECUPERAÇÃO CONTÍNUA",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.PARTICIPA_PAP)).Value,
                Texto = "[{'index': '1663', 'value': '1663 RECUPERACAO PARALELA AUTORAL PORTUGUES'}, {'index': '1664', 'value': '1664 RECUPERACAO PARALELA AUTORAL MATEMATICA'}]",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.PARTICIPA_MAIS_EDUCACAO)).Value,
                Texto = "[{'index': '1', 'value': 'XADREZ'}, {'index': '2', 'value': 'FUTEBOL'}]",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.PROJETO_FORTALECIMENTO_APRENDIZAGENS)).Value,
                Texto = "[{'index': '1255', 'value': '1255 Acompanhamento Pedagógico Matemática'}, {'index': '1204', 'value': '1204 Acompanhamento Pedagógico Português'}]",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.PROGRAMA_SAO_PAULO_INTEGRAL)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.PROGRAMA_SAO_PAULO_INTEGRAL)).Id
                                              && q.Nome == "Sim").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.HIPOTESE_ESCRITA)).Value,
                Texto = "Não alfabético",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.AVALIACOES_EXTERNAS_PROVA_SP)).Value,
                Texto = "[{'areaconhecimento': 'Ciências da Natureza', 'proficiencia': 95.5, 'nivel': 'Abaixo do básico'}, {'areaconhecimento': 'Ciências Humanas', 'proficiencia': 179.5, 'nivel': 'Básico'}]",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.OBS_AVALIACAO_PROCESSUAL)).Value,
                Texto = "OBS SOBRE A AVALIAÇÃO PROCESSUAL DO ESTUDANTE",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.FREQUENCIA)).Value,
                Texto = "",
                RespostaId = OpcoesResposta.Where(q => q.QuestaoId == Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.FREQUENCIA)).Id
                                              && q.Nome == "Frequente").FirstOrDefault().Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.RespostaMapeamentoEstudante()
            {
                QuestaoMapeamentoEstudanteId = IdsQuestoesPorNomeComponente.FirstOrDefault(pair => pair.Key.Equals(NomeComponenteQuestao.QDADE_REGISTROS_BUSCA_ATIVA)).Value,
                Texto = "10",
                RespostaId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        }

        private async Task CriarQuestoesMapeamentoEstudante(long idMapeamentoEstudanteSecao = 1)
        {
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.PARECER_CONCLUSIVO_ANO_ANTERIOR,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.PARECER_CONCLUSIVO_ANO_ANTERIOR)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.TURMA_ANO_ANTERIOR,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.TURMA_ANO_ANTERIOR)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.ANOTACOES_PEDAG_BIMESTRE_ANTERIOR)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.CLASSIFICADO,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.CLASSIFICADO)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.RECLASSIFICADO,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.RECLASSIFICADO)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.MIGRANTE,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.MIGRANTE)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.ACOMPANHADO_SRM_CEFAI,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.ACOMPANHADO_SRM_CEFAI)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.POSSUI_PLANO_AEE,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.POSSUI_PLANO_AEE)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.ACOMPANHADO_NAAPA,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.ACOMPANHADO_NAAPA)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.ACOES_REDE_APOIO,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.ACOES_REDE_APOIO)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.ACOES_RECUPERACAO_CONTINUA,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.ACOES_RECUPERACAO_CONTINUA)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.PARTICIPA_PAP,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.PARTICIPA_PAP)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.PARTICIPA_MAIS_EDUCACAO,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.PARTICIPA_MAIS_EDUCACAO)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.PROJETO_FORTALECIMENTO_APRENDIZAGENS,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.PROJETO_FORTALECIMENTO_APRENDIZAGENS)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.PROGRAMA_SAO_PAULO_INTEGRAL,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.PROGRAMA_SAO_PAULO_INTEGRAL)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.HIPOTESE_ESCRITA,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.HIPOTESE_ESCRITA)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.AVALIACOES_EXTERNAS_PROVA_SP,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.AVALIACOES_EXTERNAS_PROVA_SP)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.OBS_AVALIACAO_PROCESSUAL,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.OBS_AVALIACAO_PROCESSUAL)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.FREQUENCIA,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.FREQUENCIA)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
            IdsQuestoesPorNomeComponente.Add(NomeComponenteQuestao.QDADE_REGISTROS_BUSCA_ATIVA,
            await InserirNaBaseAsync(new Dominio.QuestaoMapeamentoEstudante()
            {
                MapeamentoEstudanteSecaoId = idMapeamentoEstudanteSecao,
                QuestaoId = Questoes.FirstOrDefault(q => q.NomeComponente.Equals(NomeComponenteQuestao.QDADE_REGISTROS_BUSCA_ATIVA)).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            }));
        }

        private async Task<long> CriarMapeamentoEstudanteSecao(long idMapeamento = 1)
        => await InserirNaBaseAsync(new Dominio.MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = SECAO_MAPEAMENTO_ESTUDANTE_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Concluido = false
            });

        private async Task<long> CriarMapeamentoEstudante()
        => await InserirNaBaseAsync(new Dominio.MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
    }
}
