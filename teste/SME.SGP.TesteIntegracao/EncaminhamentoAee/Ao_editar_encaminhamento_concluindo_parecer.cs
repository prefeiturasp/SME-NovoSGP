using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.EncaminhamentoAee.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_editar_encaminhamento_concluindo_parecer : EncaminhamentoAEETesteBase
    {
        public Ao_editar_encaminhamento_concluindo_parecer(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorUeECargoQuery, IEnumerable<FuncionarioDTO>>), typeof(ObterFuncionariosPorUeECargoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ExecutaNotificacaoConclusaoEncaminhamentoAEECommand, bool>), typeof(ExecutaNotificacaoConclusaoEncaminhamentoAEECommandHandlerFake), ServiceLifetime.Scoped));
        }

        [Theory]
        [InlineData(PerfilUsuario.PAAI)]
        [InlineData(PerfilUsuario.PAEE)]
        public async Task Ao_editar_encaminhamento_concluindo_parecer_paai_paee(PerfilUsuario perfil)
        {
            await CriarDadosBase(ObterFiltro(perfil.Name()));

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 2,
                Nome = "Concluir encaminhamento",
                Ordem = 1,
                Etapa = 3,
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
                QuestaoId = 21,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 25,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                RespostaId = 22,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                RespostaId = 25,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseConcluirEncaminhamento();
            await useCase.Executar(1);

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoAEE.Deferido);
            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.FirstOrDefault().CriadoRF.ShouldBe(USUARIO_PROFESSOR_CODIGO_RF_2222222);
            
        }

        [Fact(DisplayName = "Encaminhamento AEE - Validar apenas usuário paai ou paee pode concluir o encaminhamento")]
        public async Task Validar_apenas_usuario_paae_ou_paee_pode_concluir_o_encaminhamento()
        {
            await CriarDadosBase(ObterFiltro(ObterPerfilProfessor()));

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.Analise,
                TurmaId = TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SecaoEncaminhamentoAEE()
            {
                QuestionarioId = 2,
                Nome = "Concluir encaminhamento",
                Ordem = 1,
                Etapa = 3,
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
                QuestaoId = 21,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoAEE()
            {
                EncaminhamentoAEESecaoId = 1,
                QuestaoId = 25,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 1,
                RespostaId = 22,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoAEE()
            {
                QuestaoEncaminhamentoId = 2,
                RespostaId = 25,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ObterUseCaseConcluirEncaminhamento();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(1));

            excecao.Message.ShouldBe(MensagemNegocioEncaminhamentoAee.SOMENTE_USUARIO_PAAE_OU_PAEE_PODE_CONCLUIR_O_ENCAMINHAMENTO);
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
