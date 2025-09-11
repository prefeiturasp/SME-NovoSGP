using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.NotificacaoAlunosFaltosos
{
    public class NotificarAlunosFaltososDreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NotificarAlunosFaltososDreUseCase _useCase;

        public NotificarAlunosFaltososDreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new NotificarAlunosFaltososDreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DevePublicarComando_ParaCadaUe_QuandoFiltroForValido()
        {
            // Arrange
            var dreDto = new DreDto(132);            
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dreDto));

            var ues = new List<Ue>
            {
                new Ue { Id = 1, DreId = 123, CodigoUe = "UE1" },
                new Ue { Id = 2, DreId = 123, CodigoUe = "UE2" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUesPorDreCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUesPorDreCodigoQuery>(q => q.DreId == dreDto.DreCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoFiltroForNulo()
        {
            // Arrange            
            var mensagem = new MensagemRabbit(string.Empty);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
