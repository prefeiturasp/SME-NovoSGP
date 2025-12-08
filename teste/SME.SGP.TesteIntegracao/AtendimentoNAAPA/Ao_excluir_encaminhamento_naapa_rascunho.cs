using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_excluir_encaminhamento_naapa_rascunho : AtendimentoNAAPATesteBase
    {
        
        public Ao_excluir_encaminhamento_naapa_rascunho(CollectionFixture collectionFixture) : base(collectionFixture)
        { }


        [Fact(DisplayName = "Encaminhamento NAPA - Gestor (Ex.: Perfil CP) da Ue deve excluir encaminhamento em situação rascunho")]
        public async Task Ao_excluir_encaminhamento_rascunho()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var excluirEncaminhamentoNaapaUseCase = ObterServicoExcluirEncaminhamento();

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);
            
            await CriarEncaminhamentoNAAPA();
            
            await CriarEncaminhamentoNAAPASecao();
            
            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(dataQueixa);

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.ShouldNotBeNull();
            encaminhamentoNAAPA.Where(encaminhamento => !encaminhamento.Excluido).Count().ShouldBe(1);

            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            encaminhamentoNAAPASecao.Where(encaminhamentoSecao => !encaminhamentoSecao.Excluido).Count().ShouldBe(1);

            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Where(encaminhamentoQuestao => !encaminhamentoQuestao.Excluido).Count().ShouldBe(2);
            
            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Where(encaminhamentoResposta => !encaminhamentoResposta.Excluido).Count().ShouldBe(2);

            var retorno = await excluirEncaminhamentoNaapaUseCase.Executar(1);
            retorno.ShouldBeTrue();

            var encaminhamentoNAAPAExcluido = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPAExcluido.ShouldNotBeNull();
            encaminhamentoNAAPAExcluido.Where(encaminhamento => encaminhamento.Excluido).Count().ShouldBe(1);
            encaminhamentoNAAPAExcluido.Any(encaminhamento => !encaminhamento.Excluido).ShouldBeFalse();

            var encaminhamentoNAAPASecaoExcluida = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecaoExcluida.ShouldNotBeNull();
            encaminhamentoNAAPASecaoExcluida.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            encaminhamentoNAAPASecaoExcluida.Where(encaminhamentoSecao => encaminhamentoSecao.Excluido).Count().ShouldBe(1);
            encaminhamentoNAAPASecaoExcluida.Any(encaminhamentoSecao => !encaminhamentoSecao.Excluido).ShouldBeFalse();

            var questaoEncaminhamentoNAAPAExcluida = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPAExcluida.ShouldNotBeNull();
            questaoEncaminhamentoNAAPAExcluida.Where(encaminhamentoQuestao => encaminhamentoQuestao.Excluido).Count().ShouldBe(2);
            questaoEncaminhamentoNAAPAExcluida.Any(encaminhamentoQuestao => !encaminhamentoQuestao.Excluido).ShouldBeFalse();

            var respostaEncaminhamentoNAAPAExcluida = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPAExcluida.ShouldNotBeNull();
            respostaEncaminhamentoNAAPAExcluida.Where(encaminhamentoResposta => encaminhamentoResposta.Excluido).Count().ShouldBe(2);
            respostaEncaminhamentoNAAPAExcluida.Any(encaminhamentoResposta => !encaminhamentoResposta.Excluido).ShouldBeFalse();
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}

