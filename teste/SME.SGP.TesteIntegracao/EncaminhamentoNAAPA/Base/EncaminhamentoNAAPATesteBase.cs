using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class EncaminhamentoNAAPATesteBase : TesteBaseComuns
    {
        protected const int NORMAL = 1;
        protected const int PRIORITARIA = 2;
        public EncaminhamentoNAAPATesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase(FiltroNAAPADto filtro)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtro.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(filtro);

            if (filtro.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtro.ConsiderarAnoAnterior);

            await CriarQuestionario();
            await CriarQuestoes();
            await CriarRespostas();
            await CriarSecaoEncaminhamentoNAAPAQuestionario();
        }


        private async Task CriarSecaoEncaminhamentoNAAPAQuestionario()
        {
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = 1,
                Nome = "Informações do Estudante",
                Etapa = 1,Ordem = 1,
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

        protected async Task CriarTurmaTipoCalendario(FiltroNAAPADto filtro)
        {
            await CriarTipoCalendario(filtro.TipoCalendario, filtro.ConsiderarAnoAnterior);
            await CriarTurma(filtro.Modalidade, filtro.AnoTurma, filtro.ConsiderarAnoAnterior, tipoTurno:2);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        protected IObterEncaminhamentoNAAPAUseCase ObterServicoListagemComFiltros()
        {
            return ServiceProvider.GetService<IObterEncaminhamentoNAAPAUseCase>();    
        }
        
        protected IRegistrarEncaminhamentoNAAPAUseCase ObterServicoRegistrarEncaminhamento()
        {
            return ServiceProvider.GetService<IRegistrarEncaminhamentoNAAPAUseCase>();    
        }

        protected IExcluirEncaminhamentoNAAPAUseCase ObterServicoExcluirEncaminhamento()
        {
            return ServiceProvider.GetService<IExcluirEncaminhamentoNAAPAUseCase>();
        }
        
        protected IObterEncaminhamentoNAAPAPorIdUseCase ObterServicoObterEncaminhamentoNAAPAPorId()
        {
            return ServiceProvider.GetService<IObterEncaminhamentoNAAPAPorIdUseCase>();    
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
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 1,
                Ordem = 1,
                Nome = "Normal",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 2,
                Nome = "Prioritária",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestionario()
        {
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 1",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestoes()
        {
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Data de entrada da queixa",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Data,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 2,
                Nome = "Prioridade",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }
        protected class FiltroNAAPADto
        {
            public FiltroNAAPADto()
            {
                TipoCalendarioId = TIPO_CALENDARIO_1;
                ConsiderarAnoAnterior = false;
                CriarPeriodoEscolar = true;
            }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolar { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public int DreId { get; set; }
            public string CodigoUe { get; set; }
            public long TurmaId { get; set; }
            public int Situacao { get; set; }
            public int Prioridade { get; set; }
            public DateTime? DataAberturaQueixaInicio { get; set; }
            public DateTime? DataAberturaQueixaFim { get; set; }
        }
    }
}
