using MediatR;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.Constantes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Commands
{
    public class PublicarQuestionarioBuscaAtivaComando : IComando
    {

        private readonly TesteBase _teste;
        private const string SISTEMA_NOME = "Sistema";
        private const string SISTEMA_CODIGO_RF = "1";
        public PublicarQuestionarioBuscaAtivaComando(TesteBase testeBase)
        {
            _teste = testeBase;
        }

        public async Task Executar()
        {
            await CriarQuestionario();
            await CriarSecaoQuestionario();
            await CriarQuestoes();
            await CriarRespostas();
            await CriarRespostasComplementares();
        }

        private async Task CriarSecaoQuestionario()
        {
            await _teste.InserirNaBase(new SecaoRegistroAcaoBuscaAtiva()
            {
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Id = ConstantesQuestionarioBuscaAtiva.SECAO_REGISTRO_ACAO_ID_1,
                Nome = "Registro Ação Busca Ativa Seção 1",
                NomeComponente = ConstantesQuestionarioBuscaAtiva.SECAO_REGISTRO_ACAO_NOME_COMPONENTE,
                Etapa = 1,
                Ordem = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostasComplementares()
        {
            var opcoesResposta = _teste.ObterTodos<OpcaoResposta>();

            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP 
                                                         && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_OPCAO_RESPOSTA_SIM).FirstOrDefault();
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA 
                                                     && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA_RESPOSTA_OUTROS).FirstOrDefault();
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_1_ID_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO 
                                                     && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_VISITA_DOMICILIAR).FirstOrDefault();
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP 
                                                     && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_OPCAO_RESPOSTA_NAO).FirstOrDefault();
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP 
                                                     && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_OPCAO_RESPOSTA_SIM).FirstOrDefault();
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_5_ID_OBS_GERAL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP 
                                                     && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_OPCAO_RESPOSTA_NAO).FirstOrDefault();
            await _teste.InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = opcaoRespostaBase.Id,
                QuestaoComplementarId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_ID_OBS_GERAL_NAO_CONTATOU_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostas()
        {
            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                Ordem = 1,
                Nome = "Ligação telefonica",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                Ordem = 2,
                Nome = "Visita Domiciliar",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            foreach (var opcoesRespostas in ConstantesQuestionarioBuscaAtiva.ObterOpcoesRespostas_JUSTIFICATIVA_MOTIVO_FALTA())
                await _teste.InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                    Id = opcoesRespostas.id,
                    Ordem = (int)opcoesRespostas.id,
                    Nome = opcoesRespostas.descricao,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

            foreach (var opcoesRespostas in ConstantesQuestionarioBuscaAtiva.ObterOpcoesRespostas_QUESTOES_OBS_DURANTE_VISITA())
                await _teste.InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                    Id = opcoesRespostas.id,
                    Ordem = (int)opcoesRespostas.id,
                    Nome = opcoesRespostas.descricao,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                Ordem = 1,
                Nome = "Ligação telefonica",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                Ordem = 2,
                Nome = "Visita Domiciliar",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }


        private async Task CriarQuestionario()
        {
            await _teste.InserirNaBase(new Questionario()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Nome = "Questionário Registro Ação Busca Ativa Etapa 1 Seção 1",
                Tipo = TipoQuestionario.RegistroAcaoBuscaAtiva,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestoes()
        {
            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO,
                Obrigatorio = true,
                Tipo = TipoQuestao.Data,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 2,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_NOME_COMPONENTE_CONSEGUIU_CONTATO_RESP,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_NOME_COMPONENTE_CONSEGUIU_CONTATO_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 4,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_CONTATO_COM_RESPONSAVEL,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_NOME_COMPONENTE_CONTATO_COM_RESPONSAVEL,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_NOME_COMPONENTE_CONTATO_COM_RESPONSAVEL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_ID_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 2,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_NOME_COMPONENTE_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_NOME_COMPONENTE_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 3,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA,
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_1_ID_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_1_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_1_NOME_COMPONENTE_JUSTIFICATIVA_MOTIVO_FALTA_OUTROS,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_1_ID_QUESTOES_OBS_DURANTE_VISITA,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_1_NOME_COMPONENTE_QUESTOES_OBS_DURANTE_VISITA,
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_1_NOME_COMPONENTE_QUESTOES_OBS_DURANTE_VISITA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_5_ID_OBS_GERAL,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 5,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_5_NOME_COMPONENTE_OBS_GERAL,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_5_NOME_COMPONENTE_OBS_GERAL,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_ID_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 1,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_1_NOME_COMPONENTE_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await _teste.InserirNaBase(new Questao()
            {
                Id = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_ID_OBS_GERAL_NAO_CONTATOU_RESP,
                QuestionarioId = ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1,
                Ordem = 2,
                Nome = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_NOME_COMPONENTE_OBS_GERAL_NAO_CONTATOU_RESP,
                Obrigatorio = false,
                Tipo = TipoQuestao.Texto,
                NomeComponente = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_2_NOME_COMPONENTE_OBS_GERAL_NAO_CONTATOU_RESP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
    }
}
