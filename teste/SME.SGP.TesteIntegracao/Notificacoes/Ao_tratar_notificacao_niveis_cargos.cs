using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Notificacoes.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Notificacoes
{
    public class Ao_tratar_notificacao_niveis_cargos : TesteBaseComuns
    {
        public Ao_tratar_notificacao_niveis_cargos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpTratarNotificacaoCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosCargosPorUeCargosQuery, IEnumerable<FuncionarioCargoDTO>>), typeof(ObterFuncionariosCargosPorUeCargosQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName ="Notificações - Tratar notificações por níveis cargos")]
        public async Task Ao_tratar_notificacoes_por_niveis_cargos()
        {
            await CriarDreUePerfil();

            CriarClaimUsuario(ObterPerfilCP());

            await CriarUsuarios();

            var notificacao = new Notificacao
            {
                Codigo = 1,
                Tipo = NotificacaoTipo.Fechamento,
                Status = NotificacaoStatus.Pendente,
                Categoria = NotificacaoCategoria.Informe,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                Titulo = "Notificação Titulo",
                Mensagem = "Notificação mensagem",
                CriadoEm =
                    DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            };

            await InserirNaBase(notificacao);

            notificacao.Id.ShouldBeGreaterThan(0);

            var workflow = new WorkflowAprovacao
            {
                UeId = UE_CODIGO_1,
                DreId = DRE_CODIGO_1,
                Ano =
                    DateTimeExtension
                        .HorarioBrasilia()
                        .Year,
                NotificacaoTipo =
                    NotificacaoTipo.Fechamento,
                NotifacaoMensagem =
                    "Mensagem aprovacao",
                NotifacaoTitulo =
                    "Titulo aprovacao",
                CriadoPor = SISTEMA_NOME,
                CriadoEm = DateTime.Now,
                CriadoRF = SISTEMA_NOME,
                Tipo = WorkflowAprovacaoTipo.Basica
            };

            await InserirNaBase(workflow);
            workflow.Id.ShouldBeGreaterThan(0);

            var nivel = new WorkflowAprovacaoNivel
            {
                WorkflowId = workflow.Id,
                Status =
                    WorkflowAprovacaoNivelStatus.SemStatus,
                Cargo = Cargo.Diretor,
                Nivel = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoEm = DateTime.Now,
                CriadoRF = SISTEMA_NOME
            };

            await InserirNaBase(nivel);

            nivel.Id.ShouldBeGreaterThan(0);

            var nivelNotificacao =
                new WorkflowAprovacaoNivelNotificacao
                {
                    NotificacaoId = notificacao.Id,
                    WorkflowAprovacaoNivelId = nivel.Id
                };

            await InserirNaBase(nivelNotificacao);

            var useCase =
                ServiceProvider
                    .GetService<
                        ITrataNotificacoesNiveisCargosUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var niveis =
                ObterTodos<WorkflowAprovacaoNivel>();

            niveis.ShouldNotBeNull();
            niveis.Count.ShouldBe(2);

            niveis.Exists(
                n => n.Cargo == Cargo.Supervisor)
                .ShouldBeTrue();

            var nivelDiretor =
                niveis.Find(
                    n => n.Cargo == Cargo.Diretor);

            nivelDiretor.ShouldNotBeNull();

            nivelDiretor.Status.ShouldBe(
                WorkflowAprovacaoNivelStatus.Substituido);
        }
    }
}
