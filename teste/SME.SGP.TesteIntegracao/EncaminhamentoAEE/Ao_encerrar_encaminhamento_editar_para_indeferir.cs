using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.EncaminhamentoAee.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_encerrar_encaminhamento_editar_para_indeferir : EncaminhamentoAEETesteBase
    {
        public Ao_encerrar_encaminhamento_editar_para_indeferir(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ExecutaNotificacaoEncerramentoEncaminhamentoAEECommand, bool>), typeof(ExecutaNotificacaoEncerramentoEncaminhamentoAEECommandHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento AEE - Ao editar encaminhamento em aguardando validação enviar para indeferido pelo professor cp")]
        public async Task Ao_editar_encaminhamento_em_aguardado_validacao_enviar_para_indeferido_professor_cp()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilCP()));

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Descrição do encaminhamento",
                Ordem = 1,
                Etapa = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = "Resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseEncerrarEncaminhamento();
            await useCase.Executar(1, "Validação da coordenação");

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoAEE.Indeferido);
            encaminhamento.MotivoEncerramento.ShouldBe("Validação da coordenação");
            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.FirstOrDefault().CriadoRF.ShouldBe(USUARIO_PROFESSOR_CODIGO_RF_2222222);
        }

        [Fact(DisplayName = "Encaminhamento AEE - Validar apenas gestor escolar pode realizar indeferimento")]
        public async Task Validar_apenas_gestor_escolar_pode_realizar_indeferimento()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilProfessor()));

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Encaminhado,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 1,
                Nome = "Descrição do encaminhamento",
                Ordem = 1,
                Etapa = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoAEESecao()
            {
                EncaminhamentoAEEId = 1,
                SecaoEncaminhamentoAEEId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = "Resposta",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = string.Empty,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseEncerrarEncaminhamento();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(1, "Validação da coordenação"));

            excecao.Message.ShouldBe(MensagemNegocioEncaminhamentoAee.SOMENTE_GESTOR_ESCOLAR_PODE_REALIZAR_INDEFERIMENTO);
        }

        private FiltroAEEDto ObterFiltro(string perfil)
        {
            return new FiltroAEEDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                AnoTurma = ANO_7,
                ConsiderarAnoAnterior = false,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            };
        }
    }
}
