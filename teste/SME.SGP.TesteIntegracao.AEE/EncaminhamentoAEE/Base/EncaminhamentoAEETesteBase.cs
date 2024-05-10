using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class EncaminhamentoAEETesteBase : TesteBaseComuns
    {
        public EncaminhamentoAEETesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase(FiltroAEEDto filtro)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtro.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(filtro);

            if (filtro.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtro.ConsiderarAnoAnterior);

            if (filtro.CriarQuestionarioAlternativo)
            {
                await CriarQuestionarioAlternativo();
                await CriarQuestoesAlternativo();
                await CriarRespostasAlternativas();
                await CriarRespostasComplementaresAlternativas();
            }
            else
            {
                await CriarQuestionario();
                await CriarQuestoes();
                await CriarRespostas();    
            }

            if (filtro.CriarSecaoEncaminhamentoAeeQuestionario)
                await CriarSecaoEncaminhamentoQuestionario();

            await CriarArquivosUpload();
        }

        private async Task CriarArquivosUpload()
        {
            await InserirNaBase(new Arquivo()
            {
                Nome = "arquivo_upload_1.jpeg",
                Codigo = new Guid("27ECD7CE-D25B-46C2-9B9F-6FB7D6F49E7F"),
                Tipo = TipoArquivo.EncaminhamentoAEE,
                TipoConteudo = "image/jpeg",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Arquivo()
            {
                Nome = "arquivo_upload_2.pdf",
                Codigo = new Guid("85FF76A4-1ABF-4D1E-A287-24D2D5301486"),
                Tipo = TipoArquivo.EncaminhamentoAEE,
                TipoConteudo = "application/pdf",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Arquivo()
            {
                Nome = "arquivo_upload_3.pdf",
                Codigo = new Guid("31941CC3-93E1-49CE-8FCA-82FD9EB24E91"),
                Tipo = TipoArquivo.EncaminhamentoAEE,
                TipoConteudo = "application/pdf",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Arquivo()
            {
                Nome = "arquivo_upload_4.pdf",
                Codigo = new Guid("0B8698F1-257D-4CDA-90A5-F98E59A167AF"),
                Tipo = TipoArquivo.EncaminhamentoAEE,
                TipoConteudo = "application/pdf",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        private async Task CriarSecaoEncaminhamentoQuestionario()
        {
            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Informações escolares",
                Etapa = 1,
                Ordem = 1,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 2,
                Nome = "Descrição do encaminhamento",
                Etapa = 1,
                Ordem = 2,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 3,
                Nome = "Parecer Coordenação",
                Etapa = 2,
                Ordem = 1,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostasComplementaresAlternativas()
        {
            //Informações escolares
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 1,
                QuestaoComplementarId = 4,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 2,
                QuestaoComplementarId = 5,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 3,
                QuestaoComplementarId = 5,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 4,
                QuestaoComplementarId = 8,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 7,
                QuestaoComplementarId = 12,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 8,
                QuestaoComplementarId = 12,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 9,
                QuestaoComplementarId = 12,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 10,
                QuestaoComplementarId = 13,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 11,
                QuestaoComplementarId = 16,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 14,
                QuestaoComplementarId = 18,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 24,
                QuestaoComplementarId = 19,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 10,
                QuestaoComplementarId = 23,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 25,
                QuestaoComplementarId = 25,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 26,
                QuestaoComplementarId = 26,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });

        }

        private async Task CriarRespostasAlternativas()
        {
            //Informações escolares
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 3,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 3,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 3,
                Ordem = 3,
                Nome = "Não sei",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            //Descrição do encaminhamento						
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 7,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 7,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 7,
                Ordem = 3,
                Nome = "Não sei",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 11,
                Ordem = 1,
                Nome = "Leitura",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 11,
                Ordem = 2,
                Nome = "Escrita",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 11,
                Ordem = 3,
                Nome = "Atividades em grupo",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
                
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 11,
                Ordem = 4,
                Nome = "Outros",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 15,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 15,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 15,
                Ordem = 3,
                Nome = "Não sei",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 17,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 17,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 17,
                Ordem = 3,
                Nome = "Não sei",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 1,
                Nome = "PAP",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            }); 
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 2,
                Nome = "SRM",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            }); 
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 3,
                Nome = "Mais educação (São Paulo Integral)",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 4,
                Nome = "Imprensa Jovem",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 5,
                Nome = "Academia Estudantil de Letras (AEL)",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 6,
                Nome = "Xadrez",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 7,
                Nome = "Robótica",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 8,
                Nome = "Outros",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 24,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 24,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestoesAlternativo()
        {
            //Informações escolares
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 0,
                Nome = string.Empty,
                Tipo = TipoQuestao.InformacoesEscolares,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justificativa de ausências",
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 2,
                Nome = "Estudante está ou esteve matriculado em classe ou escola especializada",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 3,
                Nome = "Qual último período/ano em que o estudante frequentou classe ou escola especializada",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 3,
                Nome = "Qual último período/ano em que o estudante frequentou classe ou escola especializada",
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            //Descrição do encaminhamento
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Por qual(is) motivo(s) a sua unidade educacional está encaminhando o estudante ao Atendimento Educacional Especializado (AEE)?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 2,
                Nome = "O estudante tem diagnóstico e/ou laudo?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });  
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 0,
                Nome = string.Empty,
                Tipo = TipoQuestao.Upload,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });            
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 3,
                Nome = "Quais atividades escolares o estudante mais gosta de fazer?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 4,
                Nome = "O que o estudante faz que chama a sua atenção?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 5,
                Nome = "Para o estudante, quais atividades escolares são mais difíceis?",
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Por quê?",
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Por quê?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 6,
                Nome = "Diante das dificuldades apresentadas acima, quais estratégias pedagógicas foram feitas em sala de aula antes do encaminhamento ao AEE?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 7,
                Nome = "O estudante recebe algum atendimento clínico ou participa de outras atividades além da classe comum?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Detalhamento de atendimento clínico",
                Obrigatorio = true,
                Tipo = TipoQuestao.AtendimentoClinico,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 8,
                Nome = "Você acha que o estudante necessita de algum outro tipo de atendimento?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Selecione os tipos de atendimento",
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Descreva o tipo de atendimento",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 9,
                Nome = "Quais informações relevantes a este encaminhamento foram levantadas junto à família do estudante?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 10,
                Nome = "Documentos relevantes a este encaminhamento",
                Tipo = TipoQuestao.Upload,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 11,
                Nome = "Observações adicionais (se necessário)",
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 2,
                Nome = "Detalhe as atividades",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            //Parecer coordenação
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 3,
                Ordem = 1,
                Nome = "Quais mediações pedagógicas você realizou junto ao professor de classe comum antes de encaminhar o estudante ao AEE?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 3,
                Ordem = 2,
                Nome = "Além do professor de classe comum, o encaminhamento do estudante ao AEE foi discutido com outros profissionais da unidade educacional?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 3,
                Ordem = 1,
                Nome = "Quais profissionais participaram desta discussão e quais as contribuições de cada um?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            }); 
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 3,
                Ordem = 1,
                Nome = "Justifique o motivo de não haver envolvimento de outros profissionais",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 3,
                Ordem = 3,
                Nome = "Observações adicionais (se necessário)",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });   
        }

        private async Task CriarQuestionarioAlternativo()
        {
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento AEE Etapa 1 Seção 1 - Informações escolares",
                Tipo = TipoQuestionario.EncaminhamentoAEE,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento AEE Etapa 1 Seção 2 - Descrição do encaminhamento",
                Tipo = TipoQuestionario.EncaminhamentoAEE,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento AEE Etapa 1 Seção 3 - Parecer Coordenação",
                Tipo = TipoQuestionario.EncaminhamentoAEE,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarTurmaTipoCalendario(FiltroAEEDto filtro)
        {
            await CriarTipoCalendario(filtro.TipoCalendario, filtro.ConsiderarAnoAnterior);
            await CriarTurma(filtro.Modalidade, filtro.AnoTurma, filtro.ConsiderarAnoAnterior);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        protected IObterEncaminhamentosAEEUseCase ObterServicoListagemComFiltros()
        {
            return ServiceProvider.GetService<IObterEncaminhamentosAEEUseCase>();    
        }

        protected IRegistrarEncaminhamentoAEEUseCase ObterRegistrarEncaminhamentoAee()
        {
            return ServiceProvider.GetService<IRegistrarEncaminhamentoAEEUseCase>();
        }
        
        protected IVerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase ObterServicoPodeCadastrarEncaminhamentoAee()
        {
            return ServiceProvider.GetService<IVerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase>();
        }
        
        protected IDevolverEncaminhamentoUseCase ObterServicoDevolverEncaminhamentoAee()
        {
            return ServiceProvider.GetService<IDevolverEncaminhamentoUseCase>();
        }
        
        protected IExcluirEncaminhamentoAEEUseCase ObterServicoExcluirEncaminhamentoAee()
        {
            return ServiceProvider.GetService<IExcluirEncaminhamentoAEEUseCase>();
        }

        protected IObterEncaminhamentoPorIdUseCase ObterUseCaseObterEncaminhamentoPorId()
        {
            return ServiceProvider.GetService<IObterEncaminhamentoPorIdUseCase>();
        }

        protected IObterInformacoesEscolaresDoAlunoUseCase ObterUseCaseInformacoesEscolares()
        {
            return ServiceProvider.GetService<IObterInformacoesEscolaresDoAlunoUseCase>();
        }
        protected IEnviarParaAnaliseEncaminhamentoAEEUseCase ObterServicoEnviarParaAnaliseEncaminhamentoAee()
        {
            return ServiceProvider.GetService<IEnviarParaAnaliseEncaminhamentoAEEUseCase>();
        }

        protected IEncerrarEncaminhamentoAEEUseCase ObterUseCaseEncerrarEncaminhamento()
        {
            return ServiceProvider.GetService<IEncerrarEncaminhamentoAEEUseCase>();
        }

        protected IConcluirEncaminhamentoAEEUseCase ObterUseCaseConcluirEncaminhamento()
        {
            return ServiceProvider.GetService<IConcluirEncaminhamentoAEEUseCase>();
        } 

        private async Task CriarRespostasComplementares()
        {
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 1,
                QuestaoComplementarId = 15,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 2,
                QuestaoComplementarId = 15,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 3,
                QuestaoComplementarId = 15,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 8,
                QuestaoComplementarId = 17,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
          
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 11,
                QuestaoComplementarId = 18,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 21,
                QuestaoComplementarId = 19,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostas()
        {
            //1
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //2
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //3
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 3,
                Nome = "Não sei",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //4
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 6,
                Ordem = 1,
                Nome = "Leitura",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //5
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 6,
                Ordem = 2,
                Nome = "Escrita",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //6
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 6,
                Ordem = 3,
                Nome = "Atividades em grupo",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //7
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 6,
                Ordem = 4,
                Nome = "Outros",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //8
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 8,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //9
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 8,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //10
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 8,
                Ordem = 3,
                Nome = "Não Sei",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //11
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 10,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //12
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 10,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //13
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 10,
                Ordem = 2,
                Nome = "Não Sei",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //14 
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 1,
                Nome = "PAP",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //15
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 2,
                Nome = "SRM",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //16
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 3,
                Nome = "Mais educação (São Paulo Integral)",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //17
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 4,
                Nome = "Imprensa Jovem",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //18
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 5,
                Nome = "Academia Estudantil de Letras (AEL)",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //19
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 6,
                Nome = "Xadrez",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //20
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 7,
                Nome = "Robótica",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //21
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 18,
                Ordem = 8,
                Nome = "Outros",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //22
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 21,
                Ordem = 1,
                Nome = "Barreiras arquitetônicas",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //23
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 21,
                Ordem = 2,
                Nome = "Barreiras nas comunicações e na informação",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //24
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 21,
                Ordem = 3,
                Nome = "Barreiras atitudinais",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //25
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 25,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //26
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 25,
                Ordem = 1,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestionario()
        {
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento AEE Etapa 1 Seção 2",
                Tipo = TipoQuestionario.EncaminhamentoAEE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento AEE Etapa 3",
                Tipo = TipoQuestionario.EncaminhamentoAEE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestoes()
        {
            //1
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Por qual(is) motivo(s) a sua unidade educacional está encaminhando o estudante ao Atendimento Educacional Especializado (AEE)?",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //2
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 2,
                Nome = "O estudante tem diagnóstico e/ou laudo?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //3
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 3,
                Nome = "Quais atividades escolares o estudante mais gosta de fazer?",
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //4
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 4,
                Nome = "O que o estudante faz que chama a sua atenção?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //5
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justifique",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //6
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 5,
                Nome = "Para o estudante, quais atividades escolares são mais difíceis?",
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //7
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 6,
                Nome = "Diante das dificuldades apresentadas acima, quais estratégias pedagógicas foram feitas em sala de aula antes do encaminhamento ao AEE?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //8
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 7,
                Nome = "O estudante recebe algum atendimento clínico ou participa de outras atividades além da classe comum?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //9
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justifique",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //10
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 8,
                Nome = "Você acha que o estudante necessita de algum outro tipo de atendimento?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //11
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 9,
                Nome = "Quais informações relevantes a este encaminhamento foram levantadas junto à família do estudante?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //12
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 10,
                Nome = "Documentos relevantes a este encaminhamento",
                Obrigatorio = false,
                Tipo = TipoQuestao.Upload,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //13
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 11,
                Nome = "Observações adicionais (se necessário)",
                Obrigatorio = false,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //14
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 0,
                Nome = String.Empty,
                Obrigatorio = false,
                Tipo = TipoQuestao.Upload,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //15
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Por quê?",
                Obrigatorio = false,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //16
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Por quê?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //17
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Detalhamento de atendimento clínico",
                Obrigatorio = true,
                Tipo = TipoQuestao.AtendimentoClinico,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //18
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Selecione os tipos de atendimento",
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //19
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Descreva o tipo de atendimento",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //20
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 2,
                Nome = "Detalhe as atividades",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //Questionario Etapa 3
            //21
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Quais barreiras foram identificadas no contexto escolar que justificam a necessidade da oferta do AEE?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Checkbox,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //22
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Barreiras arquitetônicas (Exemplifique)",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //23
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 2,
                Nome = "Barreiras nas comunicações e na informação (Exemplifique)",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //24
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 2,
                Nome = "Barreiras atitudinais(Exemplifique)",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //25
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 2,
                Nome = "O estudante/ criança necessita do Atendimento Educacional Especializado?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //26
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Justifique a partir do estudo de caso quais critérios são elegíveis para o atendimento educacional especializado para este estudante.",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //27
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 1,
                Nome = "Uma vez que não foram identificadas barreiras no contexto escolar do estudante, quais sugestões podem contribuir para a prática pedagógica?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            //28
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 2,
                Ordem = 2,
                Nome = "Que sugestões podem ser dadas à unidade educacional para orientar a família?",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
        protected class FiltroAEEDto
        {
            public FiltroAEEDto()
            {
                TipoCalendarioId = TIPO_CALENDARIO_1;
                ConsiderarAnoAnterior = false;
                CriarPeriodoEscolar = true;
            }
            public DateTime? DataReferencia { get; set; }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public int Bimestre { get; set; }
            public string ComponenteCurricular { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolar { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public string ProfessorRf { get; set; }
            public bool CriarQuestionarioAlternativo { get; set; }
            public bool CriarSecaoEncaminhamentoAeeQuestionario { get; set; }
        }
    }
}
