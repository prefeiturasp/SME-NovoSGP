using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Pendencia.PendenciaEncaminhamentoAEE
{
    public class Ao_gerar_pendencia_encaminhamento_AEE : TesteBaseComuns
    {
        public Ao_gerar_pendencia_encaminhamento_AEE(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "GerarPendenciaCPEncaminhamentoAEECommandHandler - Deve excluir a pendencia do encaminhamento para cp se existe pendencia e gerar outra")]
        public async Task Deve_excluir_pendencia_encaminhamento_para_cp_se_existe_pendencia()
        {
            await CriarDreUePerfil();
            await CriarUsuarios();
            await CriarTurma(Modalidade.Fundamental);
            await CriarEncaminhamentoAEE();
            await CriarPendecia();
            await CriarPendenciaEncaminhamentoAEE();
            var pendenciaEncaminhamentoAEEAnterior = ObterTodos<Dominio.PendenciaEncaminhamentoAEE>();

            var mediator = ServiceProvider.GetService<IMediator>();
            await mediator.Send(new GerarPendenciaCPEncaminhamentoAEECommand(1, SituacaoAEE.Encaminhado));

            var pendenciaEncaminhamentoAEE = ObterTodos<Dominio.PendenciaEncaminhamentoAEE>();
            Assert.NotEqual(pendenciaEncaminhamentoAEEAnterior, pendenciaEncaminhamentoAEE);
        }

        private async Task CriarPendecia()
        {
            var pendencia = new Dominio.Pendencia()
            {
                Id = 1,
                Descricao = "1",
                Titulo = "Titulo",
                DescricaoHtml = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(pendencia);
        }

        private async Task CriarPendenciaEncaminhamentoAEE()
        {
            var pendenciaEncaminhamentoAee = new Dominio.PendenciaEncaminhamentoAEE()
            {
                PendenciaId = 1,
                Pendencia = new Dominio.Pendencia()
                {
                    Id = 1,
                },
                EncaminhamentoAEEId = 1,
                EncaminhamentoAEE = new EncaminhamentoAEE()
                {
                    Id = 1
                },
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(pendenciaEncaminhamentoAee);
        }

        private async Task CriarEncaminhamentoAEE()
        {
            var encaminhamentoAee = new EncaminhamentoAEE()
            {
                TurmaId = 1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_NOME_1,
                Situacao = SituacaoAEE.Encaminhado,
                ResponsavelId = 1,
                Responsavel = new Usuario()
                {
                    Id = 2,
                    Login = USUARIO_PROFESSOR_LOGIN_1111111,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                },
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Id = 1
            };
            await InserirNaBase(encaminhamentoAee);
          
            var questionario = new Questionario()
            {
                Id = 1,
                Nome = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(questionario);

            var secaoEncaminhamentoAEE = new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Encaminhamento",
                Ordem = 1,
                Etapa = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(secaoEncaminhamentoAEE);

            var encaminhamentoAEESecao = new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEE = secaoEncaminhamentoAEE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(encaminhamentoAEESecao);
           
            var questao = new Questao()
            {
                Id = 1,
                Ordem = 1,
                Questionario = questionario,
                Nome = "1",
                QuestionarioId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(questao);

            var questaoEncaminhamentoAEE = new QuestaoEncaminhamentoAEE()
            {
                Id = 1,
                QuestaoId = 1,
                EncaminhamentoAEESecaoId = 1,
                EncaminhamentoAEESecao = encaminhamentoAEESecao,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF

            };
            await InserirNaBase(questaoEncaminhamentoAEE);


            var respostaEncaminhamentoAEE = new RespostaEncaminhamentoAEE()
            {
                Id = 1,
                Texto = "1",
                QuestaoEncaminhamentoId = 1,
                QuestaoEncaminhamento = questaoEncaminhamentoAEE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(respostaEncaminhamentoAEE);
        }
    }
}
