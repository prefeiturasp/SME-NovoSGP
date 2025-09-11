using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ConciliacaoFrequenciaTurmasAlunosCronUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConciliacaoFrequenciaTurmasAlunosCronUseCase _useCase;

        public ConciliacaoFrequenciaTurmasAlunosCronUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConciliacaoFrequenciaTurmasAlunosCronUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DevePublicarMensagemNaFilaCorretamente()
        {
            // Arrange
            PublicarFilaSgpCommand capturarComando = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true)
                         .Callback<IRequest<bool>, CancellationToken>((command, token) =>
                         {
                             capturarComando = (PublicarFilaSgpCommand)command;
                         });

            // Act
            await _useCase.Executar();

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            capturarComando.Should().NotBeNull();
            capturarComando.Rota.Should().Be(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasAlunosSync);
            capturarComando.NotificarErroUsuario.Should().BeFalse();

            capturarComando.Filtros.Should().BeOfType<FiltroCalculoFrequenciaDataRereferenciaDto>();
            var filtro = (FiltroCalculoFrequenciaDataRereferenciaDto)capturarComando.Filtros;
            filtro.DataReferencia.Date.Should().Be(DateTime.Today.Date);
        }
    }
}