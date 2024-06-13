using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Notificacoes.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Notificacoes
{
    public class Ao_excluir_notificacao_periodicamente : TesteBaseComuns
    {
        public Ao_excluir_notificacao_periodicamente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpExcluirNotificacaoCommandHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Notificação - Excluir notificação de alerta lida com mais de 5 dias")]
        public async Task Ao_excluir_notificacao_alerta_lida_mais_5_dias()
        {
            await CriarDreUePerfil();
            CriarClaimUsuario(ObterPerfilCP());
            await CriarUsuarios();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "DiasExclusaoNotificacoesLidasDeAlerta",
                Descricao = "DiasExclusaoNotificacoesLidasDeAlerta",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.DiasExclusaoNotificacoesLidasDeAlerta,
                Valor = "5",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new Notificacao()
            {
                Id = 1,
                Codigo = 1,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Alerta,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-6),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            var useCase = ServiceProvider.GetService<IExecutarExclusaoNotificacoesPeriodicamenteUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.Count().ShouldBe(0);
        }

        [Fact(DisplayName = "Notificação - Excluir notificação de aviso lida com mais de 2 dias")]
        public async Task Ao_excluir_notificacao_aviso_lida_mais_2_dias()
        {
            await CriarDreUePerfil();
            CriarClaimUsuario(ObterPerfilCP());
            await CriarUsuarios();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "DiasExclusaoNotificacoesLidasDeAviso",
                Descricao = "DiasExclusaoNotificacoesLidasDeAviso",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.DiasExclusaoNotificacoesLidasDeAviso,
                Valor = "2",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new Notificacao()
            {
                Id = 1,
                Codigo = 1,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            var useCase = ServiceProvider.GetService<IExecutarExclusaoNotificacoesPeriodicamenteUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.Count().ShouldBe(0);
        }

        [Fact(DisplayName = "Notificação - Excluir notificação de aviso/alerta não lida com mais de 30 dias")]
        public async Task Ao_excluir_notificacao_aviso_alerta_nao_lida_mais_30_dias()
        {
            await CriarDreUePerfil();
            CriarClaimUsuario(ObterPerfilCP());
            await CriarUsuarios();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta",
                Descricao = "DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta,
                Valor = "30",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new Notificacao()
            {
                Codigo = 1,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Pendente,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-30),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new Notificacao()
            {
                Codigo = 2,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Pendente,
                Categoria = NotificacaoCategoria.Alerta,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-30),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            var useCase = ServiceProvider.GetService<IExecutarExclusaoNotificacoesPeriodicamenteUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.Count().ShouldBe(0);
        }


        [Fact(DisplayName = "Notificação - Excluir notificação de aviso com referência")]
        public async Task Ao_excluir_notificacao_aviso_com_referencia()
        {
            await CriarDreUePerfil();
            CriarClaimUsuario(ObterPerfilCP());
            await CriarUsuarios();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "DiasExclusaoNotificacoesLidasDeAviso",
                Descricao = "DiasExclusaoNotificacoesLidasDeAviso",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.DiasExclusaoNotificacoesLidasDeAviso,
                Valor = "2",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new Notificacao()
            {
                Id = 1,
                Codigo = 1,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Lida,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-2),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new Devolutiva()
            {
                Descricao = "Devolutiva",
                CodigoComponenteCurricular = long.Parse(COMPONENTE_CIENCIAS_ID_89),
                PeriodoFim = DateTimeExtension.HorarioBrasilia(),
                PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotificacaoDevolutiva()
            {
                NotificacaoId = 1,
                DevolutivaId = 1
            });

            await CriarTurma(Modalidade.Fundamental, ANO_1, TURMA_CODIGO_1);

            await InserirNaBase(new PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoPlanoAEE.Validado,
                AlunoNome = ALUNO_NOME_1,
                AlunoNumero = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotificacaoPlanoAEE()
            {
                NotificacaoId = 1,
                PlanoAEEId = 1,
                Tipo = NotificacaoPlanoAEETipo.PlanoCriado,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IExecutarExclusaoNotificacoesPeriodicamenteUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.Count().ShouldBe(0);
        }

        [Fact(DisplayName = "Notificação - Não excluir notificação de aviso não lida com mais de 30 dias quando for Inatividade Atendimento NAAPA")]
        public async Task Ao_excluir_notificacao_aviso_alerta_nao_deve_excluir_notificacao_inatividade_atendimento_naapa()
        {
            await CriarDreUePerfil();
            CriarClaimUsuario(ObterPerfilCP());
            await CriarUsuarios();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta",
                Descricao = "DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Tipo = TipoParametroSistema.DiasExclusaoNotificacoesNaoLidasDeAvisoEAlerta,
                Valor = "30",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new Notificacao()
            {
                Codigo = 1,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Pendente,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-30),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new Notificacao()
            {
                Codigo = 2,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Pendente,
                Categoria = NotificacaoCategoria.Alerta,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-30),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new Notificacao()
            {
                Codigo = 3,
                Tipo = NotificacaoTipo.InatividadeAtendimentoNAAPA,
                Status = NotificacaoStatus.Pendente,
                Categoria = NotificacaoCategoria.Aviso,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm = DateTimeExtension.HorarioBrasilia().AddDays(-30),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            var useCase = ServiceProvider.GetService<IExecutarExclusaoNotificacoesPeriodicamenteUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var notificacao = ObterTodos<Notificacao>();
            notificacao.ShouldNotBeNull();
            notificacao.Count().ShouldBe(1);
            notificacao.FirstOrDefault().Tipo.ShouldBe(NotificacaoTipo.InatividadeAtendimentoNAAPA);
        }
    }
}
