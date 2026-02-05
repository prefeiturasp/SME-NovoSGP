using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Informes
{
    public class ExecutarNotificacaoInformativoUsuariosUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ExecutarNotificacaoInformativoUsuariosUseCase useCase;

        public ExecutarNotificacaoInformativoUsuariosUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ExecutarNotificacaoInformativoUsuariosUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Codigo_Atual_Ja_Tem_Protecao_Contra_Duplicacao()
        {
            var informativoId = 1L;
            var usuarioRfDuplicado = "1234567";

            var mensagem = new MensagemRabbit(informativoId.ToString());

            var informativo = new Informativo
            {
                Id = informativoId,
                Titulo = "Teste Informativo",
                Texto = "Texto do informativo",
                Perfis = new List<InformativoPerfil>
                {
                    new InformativoPerfil { CodigoPerfil = 1 }
                },
                Modalidades = new List<InformativoModalidade>(),
                Dre = null,
                Ue = null
            };

            var guidPerfil = Guid.NewGuid().ToString();

            var gruposUsuarios = new List<GruposDeUsuariosDto>
            {
                new GruposDeUsuariosDto { Id = 1, GuidPerfil = Guid.Parse(guidPerfil) }
            };

            var dres = new List<Dominio.Dre>
            {
                new Dominio.Dre { CodigoDre = "123456" },
                new Dominio.Dre { CodigoDre = "789012" },
                new Dominio.Dre { CodigoDre = "345678" },
                new Dominio.Dre { CodigoDre = "901234" }
            };

            var notificacoesEnviadas = new List<NotificacaoInformativoUsuarioFiltro>();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformesPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(informativo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterGruposDeUsuariosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(gruposUsuarios);

            mediatorMock.Setup(m => m.Send(It.Is<ObterTodasDresQuery>(q => q == ObterTodasDresQuery.Instance), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRfsUsuariosPorPerfisDreUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UsuarioPerfilsAbrangenciaDto>
                {
                    new UsuarioPerfilsAbrangenciaDto { UsuarioRf = usuarioRfDuplicado }
                });

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((request, token) =>
                {
                    var command = request as PublicarFilaSgpCommand;
                    if (command?.Filtros != null && command.Filtros is NotificacaoInformativoUsuarioFiltro notificacao)
                    {
                        notificacoesEnviadas.Add(notificacao);
                    }
                })
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            Assert.Single(notificacoesEnviadas);
            Assert.Equal(usuarioRfDuplicado, notificacoesEnviadas[0].UsuarioRf);
        }

        [Fact]
        public async Task Cenario_Hipotetico_Sem_Distinct_Haveria_Duplicacao()
        {
            var informativoId = 1L;
            var usuarioRfDuplicado = "1234567";

            var mensagem = new MensagemRabbit(informativoId.ToString());

            var informativo = new Informativo
            {
                Id = informativoId,
                Titulo = "Teste Informativo",
                Texto = "Texto do informativo",
                Perfis = new List<InformativoPerfil>
                {
                    new InformativoPerfil { CodigoPerfil = 1 }
                },
                Modalidades = new List<InformativoModalidade>(),
                Dre = null,
                Ue = null
            };

            var guidPerfil = Guid.NewGuid().ToString();

            var gruposUsuarios = new List<GruposDeUsuariosDto>
            {
                new GruposDeUsuariosDto { Id = 1, GuidPerfil = Guid.Parse(guidPerfil) }
            };

            var dres = new List<Dominio.Dre>
            {
                new Dominio.Dre { CodigoDre = "123456" },
                new Dominio.Dre { CodigoDre = "789012" }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformesPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(informativo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterGruposDeUsuariosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(gruposUsuarios);

            mediatorMock.Setup(m => m.Send(It.Is<ObterTodasDresQuery>(q => q == ObterTodasDresQuery.Instance), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRfsUsuariosPorPerfisDreUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UsuarioPerfilsAbrangenciaDto>
                {
                    new UsuarioPerfilsAbrangenciaDto { UsuarioRf = usuarioRfDuplicado },
                    new UsuarioPerfilsAbrangenciaDto { UsuarioRf = usuarioRfDuplicado } // Duplicado intencional
                });

            var notificacoesEnviadas = new List<NotificacaoInformativoUsuarioFiltro>();

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((request, token) =>
                {
                    var command = request as PublicarFilaSgpCommand;
                    if (command?.Filtros != null && command.Filtros is NotificacaoInformativoUsuarioFiltro notificacao)
                    {
                        notificacoesEnviadas.Add(notificacao);
                    }
                })
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            Assert.Single(notificacoesEnviadas);
        }

        [Fact]
        public async Task Deve_Enviar_Notificacao_Apenas_Uma_Vez_Para_Usuarios_Unicos()
        {
            var informativoId = 1L;
            var usuarioRf1 = "1234567";
            var usuarioRf2 = "7654321";

            var mensagem = new MensagemRabbit(informativoId.ToString());

            var informativo = new Informativo
            {
                Id = informativoId,
                Titulo = "Teste Informativo",
                Texto = "Texto do informativo",
                Perfis = new List<InformativoPerfil>
                {
                    new InformativoPerfil { CodigoPerfil = 1 }
                },
                Modalidades = new List<InformativoModalidade>(),
                Dre = new Dominio.Dre { CodigoDre = "123456" }
            };

            var guidPerfil = Guid.NewGuid().ToString();

            var gruposUsuarios = new List<GruposDeUsuariosDto>
            {
                new GruposDeUsuariosDto { Id = 1, GuidPerfil = Guid.Parse(guidPerfil) }
            };

            var usuariosPerfils = new List<UsuarioPerfilsAbrangenciaDto>
            {
                new UsuarioPerfilsAbrangenciaDto { UsuarioRf = usuarioRf1 },
                new UsuarioPerfilsAbrangenciaDto { UsuarioRf = usuarioRf2 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformesPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(informativo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterGruposDeUsuariosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(gruposUsuarios);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRfsUsuariosPorPerfisDreUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuariosPerfils);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(
                m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()),
                Times.Exactly(2)
            );
        }
    }
}