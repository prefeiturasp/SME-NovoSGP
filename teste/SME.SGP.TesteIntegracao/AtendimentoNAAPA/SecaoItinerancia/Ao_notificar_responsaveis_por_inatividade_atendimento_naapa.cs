using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.SecaoItinerancia
{
    public class Ao_notificar_responsaveis_por_inatividade_atendimento_naapa : AtendimentoNAAPATesteBase
    {
        private const long ENCAMINHAMENTO_ID_1 = 1;
        private const long ENCAMINHAMENTO_SECAO_ID_1 = 1;
        private const long RESPOSTA_ID_1 = 1;

        public Ao_notificar_responsaveis_por_inatividade_atendimento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFakeNAAPA_Desistente_ReclassificadoSaida), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>), typeof(ObterFuncionariosDreOuUePorPerfisQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Deve notificar os responsáveis da inatividade sem atendimento")]
        public async Task Deve_notificar_responsaveis_inatividade_atendimento()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_8,
                DreId = DRE_ID_1,
                CodigoUe = UE_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.EmAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            await GerarDadosEncaminhamentoNAAPA();

            var useCase = ObterServicoNotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase();
            var dto = new AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto()
            {
                AlunoNome = ALUNO_NOME_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                EncaminhamentoId = 1,
                TurmaId = TURMA_ID_1,
                TurmaNome = TURMA_NOME_1,
                UeCodigo = UE_CODIGO_1,
                UeNome = UE_NOME_1,
                TipoEscola = TipoEscola.EMEF,
                DreCodigo = DRE_CODIGO_1,
                DreNome = DRE_NOME_1
            };

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto));

            var gerouNotificacoes = await useCase.Executar(mensagem);
            gerouNotificacoes.ShouldBe(true);

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();
            encaminhamentoNAAPA.DataUltimaNotificacaoSemAtendimento.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count.ShouldBe(4);
            notificacoes.All(n => n.Tipo == NotificacaoTipo.InatividadeAtendimentoNAAPA).ShouldBeTrue();
            var notificacao = notificacoes.FirstOrDefault();
            UtilRegex.RemoverTagsHtml(notificacao.Mensagem).ShouldBe(ObterMensagem());

            var notificacoesInatividadeAtendimento = ObterTodos<InatividadeAtendimentoNAAPANotificacao>();
            notificacoesInatividadeAtendimento.ShouldNotBeNull();
            notificacoesInatividadeAtendimento.Count.ShouldBe(4);
        }

        private string ObterMensagem()
        {
            var mensagem = new StringBuilder();
            mensagem.Append($"O Encaminhamento do estudante {ALUNO_NOME_1}({ALUNO_CODIGO_1}) ");
            mensagem.Append($"da turma {TURMA_NOME_1} da {TipoEscola.EMEF.ShortName()} {UE_NOME_1}({DRE_NOME_1}) ");
            mensagem.Append($"está há 30 dias sem registro de atendimento. ");
            mensagem.Append($"Caso não seja mais necessário acompanhamento do estudante então o encaminhamento deve ser encerrado ou deve ser registrado o acompanhamento. ");
            mensagem.Append("Clique aqui para consultar o encaminhamento.");
            return mensagem.ToString();
        }
        private async Task GerarDadosEncaminhamentoNAAPA()
        {
            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();
            await CriarQuestoesEncaminhamentoNAAPA();
            await CriarRespostasEncaminhamentoNAAPA(RESPOSTA_ID_1);
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = ALUNO_NOME_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Ativo
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = ENCAMINHAMENTO_ID_1,
                SecaoEncaminhamentoNAAPAId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = ENCAMINHAMENTO_SECAO_ID_1,
                QuestaoId = ID_QUESTAO_DATA_ENTRADA_QUEIXA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = ENCAMINHAMENTO_SECAO_ID_1,
                QuestaoId = ID_QUESTAO_PRIORIDADE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(long questaoEncaminhamentoId)
        {
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                Texto = DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId + 1,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
