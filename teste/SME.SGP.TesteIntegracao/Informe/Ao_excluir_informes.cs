using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Informe.Base;
using SME.SGP.TesteIntegracao.Informe.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Informe
{
    public class Ao_excluir_informes : InformesBase
    {
        public Ao_excluir_informes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeInforme), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Informes - Excluir informes todas dre")]
        public async Task Ao_excluir_informes_todas_dre()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informes todas dre",
                Texto = "teste",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_AD,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_ADM_COTIC,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Notificacao()
            {
                Id = NOTIFICACAO_ID_1,
                Codigo = NOTIFICACAO_ID_1,
                Tipo = NotificacaoTipo.Customizado,
                Categoria = NotificacaoCategoria.Informe,
                Titulo = "Notificação Usuário Perfil AD",
                Mensagem = "Notificação Usuário Perfil AD",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new Notificacao()
            {
                Id = NOTIFICACAO_ID_2,
                Codigo = NOTIFICACAO_ID_2,
                Tipo = NotificacaoTipo.Customizado,
                Categoria = NotificacaoCategoria.Informe,
                Titulo = "Notificação Usuário Perfil ADM COTIC",
                Mensagem = "Notificação Usuário Perfil ADM COTIC",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new InformativoNotificacao()
            {
                InformativoId = INFORME_ID_1,
                NotificacaoId = NOTIFICACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoNotificacao()
            {
                InformativoId = INFORME_ID_1,
                NotificacaoId = NOTIFICACAO_ID_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await IncluirAnexos();
            var useCase = ServiceProvider.GetService<IExcluirInformesUseCase>();

            await useCase.Executar(INFORME_ID_1);

            var informes = ObterTodos<Informativo>();
            informes.All(informe => informe.Excluido).ShouldBeTrue();

            var informesPerfil = ObterTodos<InformativoPerfil>();
            informesPerfil.All(informe => informe.Excluido).ShouldBeTrue();

            var informesNotificacao = ObterTodos<InformativoNotificacao>();
            informesNotificacao.All(informe => informe.Excluido).ShouldBeTrue();
            
            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.All(notificacao => notificacao.Excluida).ShouldBeTrue();

            var anexosInformatico = ObterTodos<InformativoAnexo>();
            anexosInformatico.Count.ShouldBe(0);

            var arquivos = ObterTodos<Arquivo>();
            arquivos.Count.ShouldBe(0);
        }


        [Fact(DisplayName = "Informes - Excluir informes por dre")]
        public async Task Ao_excluir_informes_por_dre()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                Titulo = "informes por dre",
                Texto = "teste",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_AD,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_ADM_COTIC,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Notificacao()
            {
                Id = NOTIFICACAO_ID_1,
                Codigo = NOTIFICACAO_ID_1,
                Tipo = NotificacaoTipo.Customizado,
                Categoria = NotificacaoCategoria.Informe,
                Titulo = "Notificação Usuário Perfil AD",
                Mensagem = "Notificação Usuário Perfil AD",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new Notificacao()
            {
                Id = NOTIFICACAO_ID_2,
                Codigo = NOTIFICACAO_ID_2,
                Tipo = NotificacaoTipo.Customizado,
                Categoria = NotificacaoCategoria.Informe,
                Titulo = "Notificação Usuário Perfil ADM COTIC",
                Mensagem = "Notificação Usuário Perfil ADM COTIC",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                UsuarioId = USUARIO_ID_1
            });

            await InserirNaBase(new InformativoNotificacao()
            {
                InformativoId = INFORME_ID_1,
                NotificacaoId = NOTIFICACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoNotificacao()
            {
                InformativoId = INFORME_ID_1,
                NotificacaoId = NOTIFICACAO_ID_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await IncluirAnexos();
            var useCase = ServiceProvider.GetService<IExcluirInformesUseCase>();

            await useCase.Executar(INFORME_ID_1);

            var informes = ObterTodos<Informativo>();
            informes.All(informe => informe.Excluido).ShouldBeTrue();

            var informesPerfil = ObterTodos<InformativoPerfil>();
            informesPerfil.All(informe => informe.Excluido).ShouldBeTrue();

            var informesNotificacao = ObterTodos<InformativoNotificacao>();
            informesNotificacao.All(informe => informe.Excluido).ShouldBeTrue();

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.All(notificacao => notificacao.Excluida).ShouldBeTrue();

            var anexosInformatico = ObterTodos<InformativoAnexo>();
            anexosInformatico.Count.ShouldBe(0);

            var arquivos = ObterTodos<Arquivo>();
            arquivos.Count.ShouldBe(0);
        }

        private async Task IncluirAnexos(long informativoId = INFORME_ID_1)
        {
            var idArquivo = await InserirNaBaseAsync(new Arquivo()
            {
                Codigo = Guid.NewGuid(),
                Nome = $"Arquivo - 1",
                Tipo = TipoArquivo.Informativo,
                TipoConteudo = "application/pdf",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new InformativoAnexo()
            {
                ArquivoId = idArquivo,
                InformativoId = informativoId
            });

            idArquivo = await InserirNaBaseAsync(new Arquivo()
            {
                Codigo = Guid.NewGuid(),
                Nome = $"Arquivo - 2",
                Tipo = TipoArquivo.Informativo,
                TipoConteudo = "application/pdf",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new InformativoAnexo()
            {
                ArquivoId = idArquivo,
                InformativoId = informativoId
            });
        }
    }
}
