using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAee
{
    public class Ao_enviar_para_analise_encaminhamento_AEE : EncaminhamentoAEETesteBase
    {
        private const string SISTEMA = "Sistema";

        private new const string ALUNO_CODIGO_11 = "11";

        private const long TURMA_ID = 1;

        private const string NOME_ALUNO_11 = "Nome do aluno";

        private const string DRE_NOME_10 = "Nome da DRE";
        private const string DRE_CODIGO_10 = "10";
        private const string DRE_ABREVIACAO_10 = "DRE-AB";

        private const string UE_NOME_100 = "Nome da UE";
        private const string UE_CODIGO_100 = "100";

        private const string TURMA_NOME_7P = "7P";
        private const string TURMA_CODIGO_7P = "EI-7P";
        private const string TURMA_ANO_1 = "1";

        private const string PARAMETRO_AEE_GERAR_PENDENCIA_NOME = "GerarPendenciasEncaminhamentoAEE";
        private const string PARAMETRO_AEE_GERAR_PENDENCIA_DESCRICAO = "Controle de geração de pendências para os processos do Encaminhamento AEE";
        private const string PARAMETRO_AEE_GERAR_PENDENCIA_NUMERO = "5";

        private const string USUARIO_LOGADO_RF_1111111 = "1111111";
        private const string USUARIO_LOGADO_NOME_1111111 = "USUARIO LOGADO 1111111";

        private const string USUARIO_CODIGO_RF_PAEE_1 = "PAEE_1";
        private const string USUARIO_LOGIN_PAEE_1 = "PAEE_1";
        private const string USUARIO_PAEE_1 = "USUARIO PAEE 1";

        private const string USUARIO_CODIGO_RF_PAAI_1 = "PAAI_1";
        private const string USUARIO_LOGIN_PAAI_1 = "PAAI_1";
        private const string USUARIO_PAAI_1 = "USUARIO PAAI 1";

        private const string NOME_PERFIL_CP = "CP";

        private const long ID_ENCAMINHAMENTO_AEE_NAO_EXISTENTE = 99;

        private new readonly CollectionFixture collectionFixture;

        public Ao_enviar_para_analise_encaminhamento_AEE(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            this.collectionFixture = collectionFixture ?? throw new ArgumentNullException(nameof(collectionFixture));
        }

        [Fact(DisplayName = "Encaminhamento AEE - Deve retornar mensagem de exceção quando o encaminhamento não for encontrado")]
        public async Task Deve_retornar_excecao_de_negocio_encaminhamento_nao_encontrado()
        {
            await CriarUsuarioLogadoEPerfil();
            CriarClaimFundamental();

            await CriarTurmaRegularFundamental();
            await CriarEncaminhamentoEPendencia();

            var useCase = ServiceProvider.GetService<IEnviarParaAnaliseEncaminhamentoAEEUseCase>();

            await useCase.Executar(ID_ENCAMINHAMENTO_AEE_NAO_EXISTENTE).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Encaminhamento AEE - Ao enviar para análise o encaminhamento deve gerar pendência PAAI")]
        public async Task Deve_gerar_pendencia_paai()
        {
            collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
                typeof(ObterFuncionariosPorDreEolQueryHandlerComUmPAEEFake), ServiceLifetime.Scoped));

            collectionFixture.BuildServiceProvider();

            await CriarParametrosSistema();

            await CriarUsuarioLogadoEPerfil();
            CriarClaimFundamental();

            await CriarTurmaRegularFundamental();
            await CriarEncaminhamentoEPendencia();
            await CriarResponsavelPAAI();

            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_CODIGO_RF_PAAI_1,
                Login = USUARIO_LOGIN_PAAI_1,
                Nome = USUARIO_PAAI_1,
                PerfilAtual = Perfis.PERFIL_PAAI,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            var useCase = ServiceProvider.GetService<IEnviarParaAnaliseEncaminhamentoAEEUseCase>();

            var encaminhamentoAeeId = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault().Id;

            (await useCase.Executar(encaminhamentoAeeId)).ShouldBeTrue();

            var encaminhamentoAeeAtualizado = ObterTodos<Dominio.EncaminhamentoAEE>();
            encaminhamentoAeeAtualizado.Any().ShouldBeTrue();
            encaminhamentoAeeAtualizado.Any(c => c.Situacao == Dominio.Enumerados.SituacaoAEE.Analise).ShouldBeTrue();

            var pendenciaEncaminhamentoAee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciaEncaminhamentoAee.Any().ShouldBeTrue();
        }

        [Fact(DisplayName = "Encaminhamento AEE - Ao enviar para análise o encaminhamento deve gerar pendência PAEE")]
        public async Task Deve_gerar_pendencia_paee()
        {
            collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
                typeof(ObterFuncionariosPorDreEolQueryHandlerComUmPAEEFake), ServiceLifetime.Scoped));

            collectionFixture.BuildServiceProvider();

            await CriarParametrosSistema();

            await CriarUsuarioLogadoEPerfil();
            CriarClaimFundamental();

            await CriarTurmaRegularFundamental();
            await CriarEncaminhamentoEPendencia();

            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_CODIGO_RF_PAEE_1,
                Login = USUARIO_LOGIN_PAEE_1,
                Nome = USUARIO_PAEE_1,
                PerfilAtual = Perfis.PERFIL_PAEE,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            var useCase = ServiceProvider.GetService<IEnviarParaAnaliseEncaminhamentoAEEUseCase>();

            var encaminhamentoAeeId = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault().Id;

            (await useCase.Executar(encaminhamentoAeeId)).ShouldBeTrue();

            var encaminhamentoAeeAtualizado = ObterTodos<Dominio.EncaminhamentoAEE>();
            encaminhamentoAeeAtualizado.Any().ShouldBeTrue();
            encaminhamentoAeeAtualizado.Any(c => c.Situacao == Dominio.Enumerados.SituacaoAEE.Analise).ShouldBeTrue();

            var pendenciaEncaminhamentoAee = ObterTodos<PendenciaEncaminhamentoAEE>();
            pendenciaEncaminhamentoAee.Any().ShouldBeTrue();
        }

        [Fact(DisplayName = "Encaminhamento AEE - Ao enviar para análise o encaminhamento deve gerar pendência CEFAI")]
        public async Task Deve_gerar_pendencia_cefai()
        {
            collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
                typeof(ObterFuncionariosPorDreEolQueryHandlerFake), ServiceLifetime.Scoped));

            collectionFixture.BuildServiceProvider();

            await CriarParametrosSistema();

            await CriarUsuarioLogadoEPerfil();
            CriarClaimFundamental();

            await CriarTurmaRegularFundamental();
            await CriarEncaminhamentoEPendencia();

            var useCase = ServiceProvider.GetService<IEnviarParaAnaliseEncaminhamentoAEEUseCase>();

            var encaminhamentoAeeId = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault().Id;
            (await useCase.Executar(encaminhamentoAeeId)).ShouldBeTrue();

            var encaminhamentoAtualizado = ObterTodos<Dominio.EncaminhamentoAEE>();
            encaminhamentoAtualizado.Any().ShouldBeTrue();
            encaminhamentoAtualizado.Any(a => a.Situacao == Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel).ShouldBeTrue();

            var pendenciaPerfilCEFAI = ObterTodos<PendenciaPerfil>();
            pendenciaPerfilCEFAI.Any().ShouldBeTrue();
            pendenciaPerfilCEFAI.Any(a => a.PerfilCodigo == PerfilUsuario.CEFAI).ShouldBeTrue();
        }

        [Fact(DisplayName = "Encaminhamento AEE - Ao enviar para análise o encaminhamento quando CEFAI vincula PAAI deve resolver pendência e criar pendência PAAI")]
        public async Task Ao_vincular_um_paai_pelo_cefai_ao_encaminha_pendencia_deve_ser_resolvida_e_criar_uma_nova_para_paai()
        {
            collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
            typeof(ObterFuncionariosPorDreEolQueryHandlerFake), ServiceLifetime.Scoped));

            collectionFixture.BuildServiceProvider();

            await CriarParametrosSistema();

            await CriarUsuarioLogadoEPerfil();
            CriarClaimFundamental();

            await CriarTurmaRegularFundamental();
            await CriarEncaminhamentoEPendencia();

            var useCase = ServiceProvider.GetService<IEnviarParaAnaliseEncaminhamentoAEEUseCase>();

            var encaminhamentoAeeId = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault().Id;
            (await useCase.Executar(encaminhamentoAeeId)).ShouldBeTrue();

            var encaminhamentoAtualizado = ObterTodos<Dominio.EncaminhamentoAEE>();
            encaminhamentoAtualizado.Any().ShouldBeTrue();
            encaminhamentoAtualizado.Any(a => a.Situacao == Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel).ShouldBeTrue();

            var pendenciaPerfilCEFAI = ObterTodos<PendenciaPerfil>();
            pendenciaPerfilCEFAI.Any().ShouldBeTrue();
            pendenciaPerfilCEFAI.Any(a => a.PerfilCodigo == PerfilUsuario.CEFAI).ShouldBeTrue();

            await CriarResponsavelPAAI();
            (await useCase.Executar(encaminhamentoAeeId)).ShouldBeTrue();
            var pendenciaEncaminhamento = ObterTodos<PendenciaEncaminhamentoAEE>().FirstOrDefault();
            pendenciaEncaminhamento.ShouldNotBeNull();
            pendenciaEncaminhamento.EncaminhamentoAEEId.ShouldBe(encaminhamentoAeeId);
            var pendenciaUsuario = ObterTodos<Dominio.PendenciaUsuario>();
            pendenciaUsuario.ShouldNotBeNull();
            pendenciaUsuario.Exists(pendencia => pendencia.PendenciaId == pendenciaEncaminhamento.PendenciaId).ShouldBeTrue();
        }

        [Fact(DisplayName = "Encaminhamento AEE - Ao enviar para análise o encaminhamento deve gerar pendência CP")]
        public async Task Deve_gerar_pendencia_cp()
        {
            collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
                typeof(ObterFuncionariosPorDreEolQueryHandlerComMaisDeUmPAEEFake), ServiceLifetime.Scoped));

            collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorUeECargoQuery, IEnumerable<FuncionarioDTO>>),
                typeof(ObterFuncionariosPorUeECargoQueryHandlerFake), ServiceLifetime.Scoped));

            collectionFixture.BuildServiceProvider();

            await CriarParametrosSistema();

            await CriarUsuarioLogadoEPerfil();
            CriarClaimFundamental();

            await CriarTurmaRegularFundamental();
            await CriarEncaminhamentoEPendencia();

            var useCase = ServiceProvider.GetService<IEnviarParaAnaliseEncaminhamentoAEEUseCase>();

            var encaminhamentoAeeId = ObterTodos<Dominio.EncaminhamentoAEE>().FirstOrDefault().Id;
            (await useCase.Executar(encaminhamentoAeeId)).ShouldBeTrue();

            var encaminhamentoAtualizado = ObterTodos<Dominio.EncaminhamentoAEE>();
            encaminhamentoAtualizado.Any().ShouldBeTrue();
            encaminhamentoAtualizado.Any(a => a.Situacao == Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel).ShouldBeTrue();

            var pendenciaPerfilCEFAI = ObterTodos<PendenciaPerfil>();
            pendenciaPerfilCEFAI.Any().ShouldBeTrue();
            pendenciaPerfilCEFAI.Any(a => a.PerfilCodigo == PerfilUsuario.CP).ShouldBeTrue();
        }

        private async Task CriarResponsavelPAAI()
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                SupervisorId = USUARIO_CODIGO_RF_PAAI_1,
                EscolaId = UE_CODIGO_100,
                DreId = DRE_CODIGO_10,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_CODIGO_RF_PAAI_1,
                Excluido = false,
                Tipo = (int)TipoResponsavelAtribuicao.PAAI
            });
        }

        private async Task CriarTurmaRegularFundamental()
        {
            await InserirNaBase(new Dre()
            {
                Nome = DRE_NOME_10,
                CodigoDre = DRE_CODIGO_10,
                Abreviacao = DRE_ABREVIACAO_10
            });

            await InserirNaBase(new Ue()
            {
                Nome = UE_NOME_100,
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = UE_CODIGO_100
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Nome = TURMA_NOME_7P,
                CodigoTurma = TURMA_CODIGO_7P,
                Ano = TURMA_ANO_1,
                AnoLetivo = 2022,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });
        }

        private async Task CriarEncaminhamentoEPendencia()
        {
            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID,
                AlunoCodigo = ALUNO_CODIGO_11,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
                Situacao = Dominio.Enumerados.SituacaoAEE.Encaminhado,
                AlunoNome = NOME_ALUNO_11
            });

            await InserirNaBase(new Pendencia()
            {
                Tipo = TipoPendencia.AEE,
                Descricao = $"O encaminhamento do estudante {NOME_ALUNO_11} ({ALUNO_CODIGO_11}) da turma {TURMA_CODIGO_7P} da {UE_NOME_100} ({DRE_NOME_10}) está disponível para análise. <br/><a href='https://hom-novosgp.sme.prefeitura.sp.gov.br/aee/encaminhamento/editar/0'>Clique aqui para acessar o encaminhamento.</a> <br/><br/>Esta pendência será resolvida automaticamente quando o parecer do AEE for registrado no sistema",
                Titulo = $"Encaminhamento AEE para análise - {NOME_ALUNO_11} ({ALUNO_CODIGO_11}) - {UE_NOME_100} ({DRE_NOME_10})",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 13)
            });

            await InserirNaBase(new PendenciaEncaminhamentoAEE()
            {
                EncaminhamentoAEEId = 1,
                PendenciaId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA
            });

            await InserirNaBase(new PendenciaPerfil()
            {
                PendenciaId = 1,
                PerfilCodigo = PerfilUsuario.CP,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA
            });

            await InserirNaBase(new PendenciaPerfilUsuario()
            {
                PendenciaPerfilId = 1,
                UsuarioId = 1,
                PerfilCodigo = PerfilUsuario.CP,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA
            });
        }

        private async Task CriarParametrosSistema()
        {
            await InserirNaBase(new ParametrosSistema
            {
                Nome = PARAMETRO_AEE_GERAR_PENDENCIA_NOME,
                Tipo = TipoParametroSistema.GerarPendenciasEncaminhamentoAEE,
                Descricao = PARAMETRO_AEE_GERAR_PENDENCIA_DESCRICAO,
                Valor = PARAMETRO_AEE_GERAR_PENDENCIA_NUMERO,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
                Ativo = true
            });
        }

        private async Task CriarUsuarioLogadoEPerfil()
        {
            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGADO_RF_1111111,
                Login = USUARIO_LOGADO_RF_1111111,
                Nome = USUARIO_LOGADO_NOME_1111111,
                PerfilAtual = Guid.Parse(PerfilUsuario.CP.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 240,
                Tipo = TipoPerfil.UE,
                NomePerfil = NOME_PERFIL_CP,
                CodigoPerfil = Perfis.PERFIL_CP,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA
            });
        }

        private void CriarClaimFundamental()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();

            var variaveis = new Dictionary<string, object>
            {
                { "NomeUsuario", USUARIO_LOGADO_NOME_1111111 },
                { "UsuarioLogado", USUARIO_LOGADO_RF_1111111 },
                { "RF", USUARIO_LOGADO_RF_1111111 },
                { "login", USUARIO_LOGADO_RF_1111111 },
                {
                    "Claims", new List<InternalClaim>
                    {
                        new InternalClaim { Value = USUARIO_LOGADO_RF_1111111, Type = "rf" },
                        new InternalClaim { Value = Perfis.PERFIL_CP.ToString(), Type = "perfil" }
                    }
                }
            };

            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
    }
}