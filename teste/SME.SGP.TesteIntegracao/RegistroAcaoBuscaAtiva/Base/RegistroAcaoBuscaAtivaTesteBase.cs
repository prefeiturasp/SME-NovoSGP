using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class RegistroAcaoBuscaAtivaTesteBase : TesteBaseComuns
    {
        protected const long QUESTIONARIO_REGISTRO_ACAO_ID_1 = 1;
        protected const long SECAO_REGISTRO_ACAO_ID_1 = 1;
        protected const string SECAO_REGISTRO_ACAO_NOME_COMPONENTE = "SECAO_1_REGISTRO_ACAO";

        protected const string QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO = "DATA_REGISTRO_ACAO";
        protected const string QUESTAO_2_NOME_COMPONENTE_CONSEGUIU_CONTATO_RESP = "CONSEGUIU_CONTATO_RESP";
        protected const string QUESTAO_2_1_NOME_COMPONENTE_CONTATO_COM_RESPONSAVEL = "CONTATO_COM_RESPONSAVEL";
        protected const string QUESTAO_2_2_NOME_COMPONENTE_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA = "APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA";
        protected const string QUESTAO_2_3_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA = "JUSTIFICATIVA_MOTIVO_FALTA";
        protected const string QUESTAO_2_3_1_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS = "JUSTIFICATIVA_MOTIVO_FALTA_OUTROS";
        protected const string QUESTAO_2_4_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO = "PROCEDIMENTO_REALIZADO";
        protected const string QUESTAO_2_1_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP = "PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP";
        protected const string QUESTAO_2_4_1_NOME_COMPONENTE_QUESTOES_OBS_DURANTE_VISITA = "QUESTOES_OBS_DURANTE_VISITA";
        protected const string QUESTAO_3_NOME_COMPONENTE_OBS_GERAL = "OBS_GERAL";

        protected const long QUESTAO_1_ID_DATA_REGISTRO_ACAO = 1;
        protected const long QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP = 2;
        protected const long QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL = 4;
        protected const long QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA = 5;
        protected const long QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA = 6;
        protected const long QUESTAO_2_3_1_ID_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS = 7;
        protected const long QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO = 3;
        protected const long QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA = 8;
        protected const long QUESTAO_3_ID_OBS_GERAL = 9;
        protected const long QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP = 10;

        protected const string QUESTAO_CONSEGUIU_CONTATO_RESP_RESPOSTA_SIM = "Sim";
        protected const string QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA_RESPOSTA_OUTROS = "Outros";
        protected const string QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_VISITA_DOMICILIAR = "Visita Domiciliar";
        protected const string QUESTAO_CONSEGUIU_CONTATO_RESP_RESPOSTA_NAO = "Não";
        protected const string QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_LIG_TELEFONICA = "Ligação telefonica";


        public RegistroAcaoBuscaAtivaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase(FiltroRegistroAcaoDto filtro)
        {
            await CriarDreUePerfilComponenteCurricular();
            CriarClaimUsuario(filtro.Perfil);
            await CriarUsuarios();
            await CriarTurmaTipoCalendario(filtro);


            await CriarQuestionario();
            await CriarSecaoQuestionario();
            await CriarQuestoes();
            await CriarRespostas();
            await CriarRespostasComplementares();            
        }


        private async Task CriarSecaoQuestionario()
        {
            await InserirNaBase(new SecaoRegistroAcaoBuscaAtiva()
            {
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Id = SECAO_REGISTRO_ACAO_ID_1,
                Nome = "Registro Ação Busca Ativa Seção 1",
                NomeComponente = SECAO_REGISTRO_ACAO_NOME_COMPONENTE,
                Etapa = 1, Ordem = 1,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarTurmaTipoCalendario(FiltroRegistroAcaoDto filtro)
        {
            await CriarTipoCalendario(filtro.TipoCalendario);
            if (filtro.CriarTurmaPadrao)
                await CriarTurma(filtro.Modalidade, filtro.AnoTurma);
        }

        protected IObterSecoesRegistroAcaoSecaoUseCase ObterUseCaseListagemSecoes()
        {
            return ServiceProvider.GetService<IObterSecoesRegistroAcaoSecaoUseCase>();
        }

        protected IObterQuestionarioRegistroAcaoUseCase ObterUseCaseListagemQuestionario()
        {
            return ServiceProvider.GetService<IObterQuestionarioRegistroAcaoUseCase>();
        }

        protected IRegistrarRegistroAcaoUseCase ObterUseCaseRegistroAcao()
        {
            return ServiceProvider.GetService<IRegistrarRegistroAcaoUseCase>();
        }

        protected IObterRegistroAcaoPorIdUseCase ObterUseCaseObtencaoRegistroAcao()
        {
            return ServiceProvider.GetService<IObterRegistroAcaoPorIdUseCase>();
        }

        protected IObterRegistrosAcaoCriancaEstudanteAusenteUseCase ObterUseCaseListagemRegistrosAcao_EstudantesAusentes()
        {
            return ServiceProvider.GetService<IObterRegistrosAcaoCriancaEstudanteAusenteUseCase>();
        }

        protected IExcluirRegistroAcaoUseCase ObterUseCaseExclusaoRegistroAcao()
        {
            return ServiceProvider.GetService<IExcluirRegistroAcaoUseCase>();
        }

        private async Task CriarRespostasComplementares()
        {
            var opcoesResposta = ObterTodos<OpcaoResposta>();

            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == QUESTAO_CONSEGUIU_CONTATO_RESP_RESPOSTA_SIM).FirstOrDefault(); 
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA && q.Nome == QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA_RESPOSTA_OUTROS).FirstOrDefault(); 
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = QUESTAO_2_3_1_ID_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO && q.Nome == QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_VISITA_DOMICILIAR).FirstOrDefault();
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == QUESTAO_CONSEGUIU_CONTATO_RESP_RESPOSTA_NAO).FirstOrDefault();
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostas()
        {
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                Ordem = 1,
                Nome = "Ligação telefonica",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                Ordem = 2,
                Nome = "Visita Domiciliar",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            foreach (var opcoesRespostas in ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA())
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                    Id = opcoesRespostas.id,
                    Ordem = (int)opcoesRespostas.id,
                    Nome = opcoesRespostas.descricao,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

            foreach (var opcoesRespostas in ObterOpcoesRespostas_QUESTOES_OBS_DURANTE_VISITA())
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                    Id = opcoesRespostas.id,
                    Ordem = (int)opcoesRespostas.id,
                    Nome = opcoesRespostas.descricao,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                Ordem = 1,
                Nome = "Ligação telefonica",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                Ordem = 2,
                Nome = "Visita Domiciliar",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }


        private async Task CriarQuestionario()
        {
            await InserirNaBase(new Questionario()
            {
                Id = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Nome = "Questionário Registro Ação Busca Ativa Etapa 1 Seção 1",
                Tipo = TipoQuestionario.RegistroAcaoBuscaAtiva,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

        }

        private async Task CriarQuestoes()
        {
            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO,
                Obrigatorio = true,
                Tipo = TipoQuestao.Data,
                NomeComponente = QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 2,
                Nome = QUESTAO_2_NOME_COMPONENTE_CONSEGUIU_CONTATO_RESP,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = QUESTAO_2_NOME_COMPONENTE_CONSEGUIU_CONTATO_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 4,
                Nome = QUESTAO_2_4_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = QUESTAO_2_4_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = QUESTAO_2_1_NOME_COMPONENTE_CONTATO_COM_RESPONSAVEL,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = QUESTAO_2_1_NOME_COMPONENTE_CONTATO_COM_RESPONSAVEL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 2,
                Nome = QUESTAO_2_2_NOME_COMPONENTE_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = QUESTAO_2_2_NOME_COMPONENTE_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 3,
                Nome = QUESTAO_2_3_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA,
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                NomeComponente = QUESTAO_2_3_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_3_1_ID_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = QUESTAO_2_3_1_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                NomeComponente = QUESTAO_2_3_1_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = QUESTAO_2_4_1_NOME_COMPONENTE_QUESTOES_OBS_DURANTE_VISITA,
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                NomeComponente = QUESTAO_2_4_1_NOME_COMPONENTE_QUESTOES_OBS_DURANTE_VISITA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_3_ID_OBS_GERAL,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 3,
                Nome = QUESTAO_3_NOME_COMPONENTE_OBS_GERAL,
                Obrigatorio = false,
                Tipo = TipoQuestao.Texto,
                NomeComponente = QUESTAO_3_NOME_COMPONENTE_OBS_GERAL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new Questao()
            {
                Id = QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                QuestionarioId = QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = QUESTAO_2_1_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = QUESTAO_2_1_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected IEnumerable<(long id, string descricao)> ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA()
        {
            var opcoesRespostas = new List<(long id, string descricao)>();
            opcoesRespostas.Add((1, "Ausência por gripe ou resfriado (tosse, febre, dor de garganta)"));
            opcoesRespostas.Add((2, "Ausência por enjôo, diarreia, vômito"));
            opcoesRespostas.Add((3, "Ausência por doenças crônicas como anemia, diabetes, câncer, problemas cardíacos ou neurológicos, convulsões ou transplantados"));
            opcoesRespostas.Add((4, "Ausência por questões de diagnóstico de transtorno mental ou em sofrimento psíquico (depressão, ansiedade)"));
            opcoesRespostas.Add((5, "Ausência por deficiência que impeça ou dificulte o acesso e permanência à Unidade Educacional"));
            opcoesRespostas.Add((6, "Ausência do adolescente por motivo de cumprimento de medidas socioeducativas em regime fechado"));
            opcoesRespostas.Add((7, "Ausência do adolescente por motivo de cumprimento de medidas socioeducativas em casa"));
            opcoesRespostas.Add((8, "Ausência por estarem viajando no período"));
            opcoesRespostas.Add((9, "Ausência porque mora distante da escola e apresente dificuldades no deslocamento"));
            opcoesRespostas.Add((10, "Ausência por estarem cuidando de irmãos, pais ou avós"));
            opcoesRespostas.Add((11, "Ausência por motivo de falecimento"));
            opcoesRespostas.Add((12, "Há suspeita de ausência por estar realizando trabalho infantil"));
            opcoesRespostas.Add((13, "Ausência por motivo de gravidez da estudante"));
            opcoesRespostas.Add((14, "Ausência por relato do estudante que não deseja voltar para a escola"));
            opcoesRespostas.Add((15, "Ausência por não ter material escolar/uniforme"));
            opcoesRespostas.Add((16, "Ausência por falta de transporte escolar"));
            opcoesRespostas.Add((17, "Ausência por negligência da família sobre a frequência escolar (não sabe/não se preocupa/não se importa)"));
            opcoesRespostas.Add((18, "Ausência por estar em situação de rua ou na rua"));
            opcoesRespostas.Add((19, "Ausência por enfrentar dificuldades financeiras"));
            opcoesRespostas.Add((20, "Ausência por não ter moradia fixa"));
            opcoesRespostas.Add((21, "Ausência por ter sido vitima de preconceito, discriminação ou bullyng na unidade educacional"));
            opcoesRespostas.Add((22, "Ausência pelo estudante estar em luto"));
            opcoesRespostas.Add((23, "Ausência por não haver um responsável para levar para a escola"));
            opcoesRespostas.Add((24, "Ausência por ter perdido a vaga"));
            opcoesRespostas.Add((25, "Ausência devido aos seus responsáveis serem pessoas com deficiência e/ou apresentarem problemas de saúde mental ou dependência química (alcoolismo, drogas, medicamentos)"));
            opcoesRespostas.Add((26, "Ausência porque os responsáveis não querem levar o bebê/criança/adolescente para a unidade educacional"));
            opcoesRespostas.Add((27, "Ausência por envolvimento do estudante com ácool, drogas ou medicamentos"));
            opcoesRespostas.Add((28, "Ausência devido a violência do território (comunidade, bairro)"));
            opcoesRespostas.Add((29, "Outros"));
            return opcoesRespostas;
        }

        protected IEnumerable<(long id, string descricao)> ObterOpcoesRespostas_QUESTOES_OBS_DURANTE_VISITA()
        {
            var opcoesRespostas = new List<(long id, string descricao)>();
            opcoesRespostas.Add((1, "Há suspeita de negligência"));
            opcoesRespostas.Add((2, "Há suspeita de violência física"));
            opcoesRespostas.Add((3, "Há suspeita/relato de violência sexual"));
            return opcoesRespostas;
        }

        protected class FiltroRegistroAcaoDto
        {
            public FiltroRegistroAcaoDto()
            {
                CriarTurmaPadrao = true;
            }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public string AnoTurma { get; set; }
            public bool CriarTurmaPadrao { get; set; }
        }

        protected async Task GerarDadosRegistroAcao_3PrimeirasQuestoes(DateTime dataRegistro)
        {
            await CriarRegistroAcao();
            var registrosAcaoId = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>().Max(ra => ra.Id);
            await CriarRegistroAcaoSecao(registrosAcaoId);
            var registrosAcaoSecaoId = ObterTodos<Dominio.RegistroAcaoBuscaAtivaSecao>().Max(ra => ra.Id);
            await CriarQuestoesRegistroAcao(registrosAcaoSecaoId);
            var registrosAcaoQuestaoId = ObterTodos<Dominio.QuestaoRegistroAcaoBuscaAtiva>().Where(ra => ra.RegistroAcaoBuscaAtivaSecaoId == registrosAcaoSecaoId).Min(ra => ra.Id);
            await CriarRespostasRegistroAcao(dataRegistro, registrosAcaoQuestaoId);
        }

        private async Task CriarRespostasRegistroAcao(DateTime dataRegistro, long idRegistroAcaoQuestao = 1)
        {
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                Texto = dataRegistro.ToString("yyyy-MM-dd"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            idRegistroAcaoQuestao++;

            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == QUESTAO_CONSEGUIU_CONTATO_RESP_RESPOSTA_SIM).FirstOrDefault();
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                RespostaId = opcaoRespostaBase.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            idRegistroAcaoQuestao++;

            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                Texto = "OBS GERAL",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesRegistroAcao(long idRegistroAcaoSecao = 1)
        {
            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                QuestaoId = QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                QuestaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                QuestaoId = QUESTAO_3_ID_OBS_GERAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRegistroAcaoSecao(long idRegistroAcao = 1)
        {
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtivaSecao()
            {
                RegistroAcaoBuscaAtivaId = idRegistroAcao,
                SecaoRegistroAcaoBuscaAtivaId = SECAO_REGISTRO_ACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Concluido = false
            });
        }

        private async Task CriarRegistroAcao()
        {
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtiva()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
