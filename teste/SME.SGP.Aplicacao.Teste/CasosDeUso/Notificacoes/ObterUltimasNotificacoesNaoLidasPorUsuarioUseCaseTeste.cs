using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacoes
{
    public class ObterUltimasNotificacoesNaoLidasPorUsuarioUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IObterUltimasNotificacoesNaoLidasPorUsuarioUseCase _useCase;

        public ObterUltimasNotificacoesNaoLidasPorUsuarioUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterUltimasNotificacoesNaoLidasPorUsuarioUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Possui_Codigo_Rf_Deve_Usar_Rf_Na_Query()
        {
            var usuario = new Usuario { CodigoRf = "RF123", Login = "LOGIN456" };
            var notificacoesEsperadas = new List<NotificacaoBasicaDto>();
            var anoAtual = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUltimasNotificacoesNaoLidasPorUsuarioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(notificacoesEsperadas);

            var resultado = await _useCase.Executar(tituloReduzido: true);

            resultado.Should().BeEquivalentTo(notificacoesEsperadas);
            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterUltimasNotificacoesNaoLidasPorUsuarioQuery>(q =>
                    q.AnoLetivo == anoAtual &&
                    q.CodigoRf == usuario.CodigoRf &&
                    q.TituloReduzido),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nao_Possui_Codigo_Rf_Deve_Usar_Login_Na_Query()
        {
            var usuario = new Usuario { CodigoRf = null, Login = "LOGIN456" };
            var notificacoesEsperadas = new List<NotificacaoBasicaDto>();
            var anoAtual = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUltimasNotificacoesNaoLidasPorUsuarioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(notificacoesEsperadas);

            var resultado = await _useCase.Executar(tituloReduzido: false);

            resultado.Should().BeEquivalentTo(notificacoesEsperadas);
            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterUltimasNotificacoesNaoLidasPorUsuarioQuery>(q =>
                    q.AnoLetivo == anoAtual &&
                    q.CodigoRf == usuario.Login &&
                    !q.TituloReduzido),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
