using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_obter_encaminhamento_itinerancia : EncaminhamentoNAAPATesteBase
    {
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

            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 10,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
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

            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_GRUPO_FOCAL_TIPO_ATENDIMENTO_EXCLUIDO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 10,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
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
            var respostaQuestao = questao.Resposta.FirstOrDefault();
            respostaQuestao.ShouldNotBeNull();
            respostaQuestao.OpcaoRespostaId.ShouldBe(ID_GRUPO_FOCAL_TIPO_ATENDIMENTO_EXCLUIDO);
        }
    }
}
