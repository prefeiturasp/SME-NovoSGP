using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.SecaoItinerancia
{
    public class Ao_obter_encaminhamento_itinerancia : AtendimentoNAAPATesteBase
    {
        private const string PROFISSIONAIS_ENVOLVIDOS_02 = "[{\"login\": \"11223344\", \"nome\": \"psicopedagogo 02\"},{\"login\": \"55667788\", \"nome\": \"psicólogo 02\"},{\"login\": \"66778899\", \"nome\": \"coordenador naapa 01\"},{\"login\": \"77889900\", \"nome\": \"assistente social 01\"}]";
        private const string PROFISSIONAIS_ENVOLVIDOS_01 = "[{\"login\": \"11223344\", \"nome\": \"psicopedagogo 01\"},{\"login\": \"55667788\", \"nome\": \"psicólogo 01\"},{\"login\": \"66778899\", \"nome\": \"coordenador naapa 01\"},{\"login\": \"77889900\", \"nome\": \"assistente social 01\"}]";

        public Ao_obter_encaminhamento_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Obter questionário do encaminhamento itinerário NAAPA (pergunta) ")]
        public async Task Ao_obter_questionario_encaminhamento_pergunta()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterServicoObterQuestionarioItinerarioEncaminhamentoNAAPA();

            var retorno = await useCase.Executar(3, null);
            retorno.ShouldNotBeNull();

            var questoes = retorno.Questoes;
            questoes.ShouldNotBeNull();

            var questao1 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_DATA_ATENDIMENTO);
            questao1.ShouldNotBeNull();

            var questao2 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_TIPO_ATENDIMENTO);
            questao2.ShouldNotBeNull();

            var questao3 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_PROCEDIMENTO_TRABALHO);
            questao3.ShouldNotBeNull();

            var questao4 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_DESCRICAO_ATENDIMENTO);
            questao4.ShouldNotBeNull();

            var questao5 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS);
            questao5.ShouldNotBeNull();

            var questao6 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_ANEXOS_ITINERANCIA);
            questao6.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Obter questionário do encaminhamento itinerário NAAPA (pergunta e resposta) ")]
        public async Task Ao_obter_questionario_encaminhamento_pergunta_resposta()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });


            var QuestaoEncaminhamentoId = 1;

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = "Resposta",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = PROFISSIONAIS_ENVOLVIDOS_01,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterServicoObterQuestionarioItinerarioEncaminhamentoNAAPA();

            var retorno = await useCase.Executar(3, 1);
            retorno.ShouldNotBeNull();

            var questoes = retorno.Questoes;
            questoes.ShouldNotBeNull();

            var questao1 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_DATA_ATENDIMENTO);
            questao1.ShouldNotBeNull();
            questao1.Resposta.ShouldNotBeNull();
            var respostaQuestao1 = questao1.Resposta.FirstOrDefault();
            respostaQuestao1.ShouldNotBeNull();
            respostaQuestao1.Texto.ShouldBe(dataQueixa.ToString("dd/MM/yyyy"));

            var questao2 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_TIPO_ATENDIMENTO);
            questao2.ShouldNotBeNull();
            questao2.Resposta.ShouldNotBeNull();
            var respostaQuestao2 = questao2.Resposta.FirstOrDefault();
            respostaQuestao2.ShouldNotBeNull();
            respostaQuestao2.OpcaoRespostaId.ShouldBe(ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL);

            var questao3 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_PROCEDIMENTO_TRABALHO);
            questao3.ShouldNotBeNull();
            questao3.Resposta.ShouldNotBeNull();
            var respostaQuestao3 = questao3.Resposta.FirstOrDefault();
            respostaQuestao3.ShouldNotBeNull();
            respostaQuestao3.OpcaoRespostaId.ShouldBe(ID_OPCAO_RESPOSTA_ACOES_LUDICAS);

            var questao4 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_DESCRICAO_ATENDIMENTO);
            questao4.ShouldNotBeNull();
            questao4.Resposta.ShouldNotBeNull();
            var respostaQuestao4 = questao4.Resposta.FirstOrDefault();
            respostaQuestao4.ShouldNotBeNull();
            respostaQuestao4.Texto.ShouldBe("Resposta");

            var questao5 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS);
            questao5.ShouldNotBeNull();
            questao5.Resposta.ShouldNotBeNull();
            var respostaQuestao5 = questao5.Resposta.FirstOrDefault();
            respostaQuestao5.ShouldNotBeNull();
            respostaQuestao5.Texto.ShouldBe(PROFISSIONAIS_ENVOLVIDOS_01);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Obter questionário do encaminhamento itinerário NAAPA com respostas excluídas ")]
        public async Task Ao_obter_questionario_encaminhamento_com_resposta_excluidas()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });


            var QuestaoEncaminhamentoId = 1;

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_GRUPO_FOCAL_TIPO_ATENDIMENTO_EXCLUIDO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 10,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = "Resposta",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterServicoObterQuestionarioItinerarioEncaminhamentoNAAPA();

            var retorno = await useCase.Executar(3, 1);
            retorno.ShouldNotBeNull();
            var questoes = retorno.Questoes;
            questoes.ShouldNotBeNull();
            var questao = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_TIPO_ATENDIMENTO);
            questao.ShouldNotBeNull();
            questao.Resposta.ShouldNotBeNull();
            //Atendimentos com opção inativada, estes devem continuar apresentando
            var respostaQuestao = questao.Resposta.FirstOrDefault();
            respostaQuestao.ShouldNotBeNull();
            respostaQuestao.OpcaoRespostaId.ShouldBe(ID_GRUPO_FOCAL_TIPO_ATENDIMENTO_EXCLUIDO);
        }


        [Fact(DisplayName = "Encaminhamento NAAPA - Obter questionário do encaminhamento itinerário NAAPA com novas opções respostas ")]
        public async Task Ao_obter_questionario_encaminhamento_com_novas_opcoes_resposta()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });


            var QuestaoEncaminhamentoId = 1;

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ITINERANCIA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_REUNIAO_COMPARTILHDA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_REUNIAO_REDE_MARCRO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO_UE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_REUNIAO_HORARIOS_COLETIVOS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 10,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = "Resposta",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterServicoObterQuestionarioItinerarioEncaminhamentoNAAPA();

            var retorno = await useCase.Executar(3, 1);
            retorno.ShouldNotBeNull();

            //Seção renomeada
            var secao = ObterTodos<SecaoEncaminhamentoNAAPA>().FirstOrDefault(secao => secao.Id == ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA);
            secao.ShouldNotBeNull();
            secao.Nome.ShouldBe("Apoio e Acompanhamento");

            var questoes = retorno.Questoes;
            questoes.ShouldNotBeNull();

            var questao = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_TIPO_ATENDIMENTO);
            questao.ShouldNotBeNull();
            questao.Obrigatorio.ShouldBeTrue();
            //alteração nome questão tipo atendimento
            questao.Nome.ShouldBe("Modalidade de atenção");
            //opções de resposta disponíveis
            var opcoesRespostaModalidade = new long[]
            {
                   ID_OPCAO_RESPOSTA_ITINERANCIA,
                   ID_OPCAO_RESPOSTA_GRUPO_TRAB_NAAPA,
                   ID_OPCAO_RESPOSTA_ATENDIMENTO_PEDAGOGICO_DOMICILIAR,
                   ID_OPCAO_RESPOSTA_ATENDIMENTO_ATENDIMENTO_PRESENCIAL_DRE,
                   ID_OPCAO_RESPOSTA_ATENDIMENTO_REMOTO
            };
            var opcoesRespostaQuestaoModalidade = questao.OpcaoResposta.Select(opcao => opcao.Id).ToList();
            opcoesRespostaQuestaoModalidade.Except(opcoesRespostaModalidade.ToList()).Any().ShouldBeFalse();
            //validando resposta
            questao.Resposta.ShouldNotBeNull();
            var respostaQuestao = questao.Resposta.FirstOrDefault();
            respostaQuestao.ShouldNotBeNull();
            respostaQuestao.OpcaoRespostaId.ShouldBe(ID_OPCAO_RESPOSTA_ITINERANCIA);

            //Procedimento de trabalho
            var questao2 = questoes.FirstOrDefault(questao => questao.Id == ID_QUESTAO_PROCEDIMENTO_TRABALHO);
            questao2.ShouldNotBeNull();
            questao2.TipoQuestao.ShouldBe(TipoQuestao.ComboMultiplaEscolha);
            questao2.Obrigatorio.ShouldBeTrue();
            //validando opções de resposta
            var idsOpcoesRespostaProcedimentoTrabalho = new long[] {
                ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                ID_OPCAO_RESPOSTA_ANALISE_DOCUMENTAL,
                ID_OPCAO_RESPOSTA_ATENDIMENTO_REMOTO,
                ID_OPCAO_RESPOSTA_ENTREVISTA,
                ID_OPCAO_RESPOSTA_GRUPO_FOCAL,
                ID_OPCAO_RESPOSTA_REFLEXIVO_INTERVENTIVO,
                ID_OPCAO_RESPOSTA_OBSERVACAO,
                ID_OPCAO_RESPOSTA_PROJETO_TECER,
                ID_OPCAO_RESPOSTA_REUNIAO_COMPARTILHDA,
                ID_OPCAO_RESPOSTA_REUNIAO_REDE_MARCRO,
                ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO,
                ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO_UE,
                ID_OPCAO_RESPOSTA_REUNIAO_HORARIOS_COLETIVOS,
                ID_OPCAO_RESPOSTA_VISITA_TECNICA,
                ID_OPCAO_RESPOSTA_OUTRO_PROCEDIMENTO
            };
            var idsOpcaoRespostaConsulta = questao2.OpcaoResposta.Select(opcao => opcao.Id).ToList();
            idsOpcaoRespostaConsulta.Except(idsOpcoesRespostaProcedimentoTrabalho.ToList()).Any().ShouldBeFalse();
            //validando resposta
            questao2.Resposta.ShouldNotBeNull();
            var respostaQuestaoIds = questao2.Resposta.Select(resposta => resposta.OpcaoRespostaId.GetValueOrDefault()).ToList();
            respostaQuestaoIds.ShouldNotBeNull();
            var idsRespostas = new long[] {
                ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                ID_OPCAO_RESPOSTA_REUNIAO_COMPARTILHDA,
                ID_OPCAO_RESPOSTA_REUNIAO_REDE_MARCRO,
                ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO,
                ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO_UE,
                ID_OPCAO_RESPOSTA_REUNIAO_HORARIOS_COLETIVOS
            };
            respostaQuestaoIds.Except(idsRespostas.ToList()).Any().ShouldBeFalse();
        }
    }
}
