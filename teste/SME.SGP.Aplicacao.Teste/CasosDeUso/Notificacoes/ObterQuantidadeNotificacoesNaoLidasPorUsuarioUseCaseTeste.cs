using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacoes
{
    public class ObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase _useCase;

        public ObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Obter_Usuario_E_Retornar_Contagem()
        {
            var usuarioRf = "1234567";
            var quantidadeEsperada = 10;
            var anoAtual = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoRFQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuarioRf);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(quantidadeEsperada);

            var resultado = await _useCase.Executar();

            resultado.Should().Be(quantidadeEsperada);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery>(q =>
                    q.AnoLetivo == anoAtual &&
                    q.CodigoRf == usuarioRf),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
