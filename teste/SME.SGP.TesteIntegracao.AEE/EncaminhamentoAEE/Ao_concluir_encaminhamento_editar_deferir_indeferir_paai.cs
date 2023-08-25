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
using SME.SGP.TesteIntegracao.ServicosFake;
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
    public class Ao_concluir_encaminhamento_editar_deferir_indeferir_paai : EncaminhamentoAEETesteBase
    {
        public Ao_concluir_encaminhamento_editar_deferir_indeferir_paai(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorUeECargoQuery, IEnumerable<FuncionarioDTO>>), typeof(ObterFuncionariosPorUeECargoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ExecutaNotificacaoConclusaoEncaminhamentoAEECommand, bool>), typeof(ExecutaNotificacaoConclusaoEncaminhamentoAEECommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoPaai4444444QueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_editar_encaminhamento_concluindo_parecer_paai_para_deferir()
        {
            await CriarDadosBase(ObterFiltro(PerfilUsuario.PAAI.Name()));

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

            notificacao.FirstOrDefault().CriadoRF.ShouldBe(USUARIO_LOGIN_PAAI);
            
        }
        
        [Fact]
        public async Task Ao_editar_encaminhamento_concluindo_parecer_paai_paee_resposta_nao_para_indefirir()
        {
            await CriarDadosBase(ObterFiltro(PerfilUsuario.PAAI.Name()));

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
                RespostaId = 26,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            CriarPendenciasEncaminhamento(PerfilUsuario.PAAI);

            var pendenciasPaee = ObterTodos<Pendencia>();
            pendenciasPaee.ShouldNotBeNull();
            (pendenciasPaee.FirstOrDefault().Tipo == TipoPendencia.AEE).ShouldBeTrue();
            
            var pendenciasEncaminhamentosPaee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciasEncaminhamentosPaee.ShouldNotBeNull();
            pendenciasEncaminhamentosPaee.Count.ShouldBe(1);
            
            var pendenciaPerfils = ObterTodos<PendenciaPerfil>();
            pendenciaPerfils.ShouldNotBeNull();
            pendenciaPerfils.Count.ShouldBe(1);
            
            var pendenciaPerfilUsuarios = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilUsuarios.ShouldNotBeNull();
            pendenciaPerfilUsuarios.Count.ShouldBe(1);
            
            var useCase = ObterUseCaseConcluirEncaminhamento();
            await useCase.Executar(1);

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoAEE.Indeferido);
            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.FirstOrDefault().CriadoRF.ShouldBe(USUARIO_LOGIN_PAAI);
            
            pendenciasPaee = ObterTodos<Pendencia>();
            pendenciasPaee.ShouldNotBeNull();
            (pendenciasPaee.FirstOrDefault().Tipo == TipoPendencia.AEE).ShouldBeTrue();
            pendenciasPaee.FirstOrDefault().Excluido.ShouldBeTrue();
            
            pendenciasEncaminhamentosPaee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciasEncaminhamentosPaee.ShouldNotBeNull();
            pendenciasEncaminhamentosPaee.Count.ShouldBe(0);
            
            pendenciaPerfils = ObterTodos<PendenciaPerfil>();
            pendenciaPerfils.ShouldNotBeNull();
            pendenciaPerfils.Count.ShouldBe(0);
            
            pendenciaPerfilUsuarios = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilUsuarios.ShouldNotBeNull();
            pendenciaPerfilUsuarios.Count.ShouldBe(0);
        }

        private async Task CriarPendenciasEncaminhamento(PerfilUsuario perfilusuario)
        {
            await InserirNaBase(new Pendencia()
            {
                Titulo = "Encaminhamento AEE para análise",
                Descricao = "O encaminhamento do estudante NOME DO ESTUDANTE 01 da turma TURMA_001 da ESCOLA ABC está disponível para atribuição de um PAEE. <br/><a href='https://novosgp.sme.prefeitura.sp.gov.br/aee/encaminhamento/editar/0001'>Clique aqui para acessar o encaminhamento.</a> <br/><br/>Esta pendência será resolvida automaticamente quando o PAEE for atribuído no encaminhamento.",
                Tipo = TipoPendencia.AEE,
                Situacao = SituacaoPendencia.Pendente,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PendenciaEncaminhamentoAEE()
            {
                EncaminhamentoAEEId = 1,
                PendenciaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PendenciaPerfil()
            {
                PerfilCodigo = perfilusuario,
                PendenciaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new PendenciaPerfilUsuario()
            {
                PendenciaPerfilId = 1,
                UsuarioId = 1,
                PerfilCodigo = perfilusuario,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
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
