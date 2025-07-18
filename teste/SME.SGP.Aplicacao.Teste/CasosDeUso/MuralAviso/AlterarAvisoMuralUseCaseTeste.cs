using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.MuralAviso
{
    public class AlterarAvisoMuralUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AlterarAvisoMuralUseCase _useCase;

        public AlterarAvisoMuralUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AlterarAvisoMuralUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarComandoAlterarAvisoDoMuralComSucesso()
        {
            long avisoId = 1;
            string novaMensagem = "Nova mensagem do aviso.";

            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAvisoDoMuralCommand>(), default(CancellationToken)))
                .Returns(Task.FromResult(Unit.Value));

            await _useCase.Executar(avisoId, novaMensagem);

            _mediatorMock.Verify(m => m.Send(
                It.Is<AlterarAvisoDoMuralCommand>(cmd => cmd.Id == avisoId && cmd.Mensagem == novaMensagem),
                default(CancellationToken)
            ), Times.Once);
        }

        [Fact]
        public async Task Executar_DevePropagarExcecaoQuandoComandoFalhar()
        {
            long avisoId = 2;
            string novaMensagem = "Mensagem que causará erro.";
            var excecaoEsperada = new Exception("Erro simulado ao alterar aviso.");

            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAvisoDoMuralCommand>(), default(CancellationToken)))
                .ThrowsAsync(excecaoEsperada);

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(avisoId, novaMensagem));
            Assert.Equal(excecaoEsperada.Message, ex.Message);

            _mediatorMock.Verify(m => m.Send(
                It.Is<AlterarAvisoDoMuralCommand>(cmd => cmd.Id == avisoId && cmd.Mensagem == novaMensagem),
                default(CancellationToken)
            ), Times.Once);
        }

        [Theory]
        [InlineData(0, "Mensagem válida")]
        [InlineData(-1, "Mensagem válida")]
        public async Task Executar_DeveDispararValidacaoParaIdInvalido(long avisoId, string mensagem)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAvisoDoMuralCommand>(), default(CancellationToken)))
                .ThrowsAsync(new FluentValidation.ValidationException("O id do aviso deve ser informado para alteração"));

            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _useCase.Executar(avisoId, mensagem));
            Assert.Contains("O id do aviso deve ser informado para alteração", ex.Message);

            _mediatorMock.Verify(m => m.Send(
                It.Is<AlterarAvisoDoMuralCommand>(cmd => cmd.Id == avisoId && cmd.Mensagem == mensagem),
                default(CancellationToken)
            ), Times.Once);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(1, "")]
        [InlineData(1, " ")]
        public async Task Executar_DeveDispararValidacaoParaMensagemInvalida(long avisoId, string mensagem)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAvisoDoMuralCommand>(), default(CancellationToken)))
                .ThrowsAsync(new FluentValidation.ValidationException("A mensagem do aviso deve ser informada para alteração"));

            var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _useCase.Executar(avisoId, mensagem));
            Assert.Contains("A mensagem do aviso deve ser informada para alteração", ex.Message);

            _mediatorMock.Verify(m => m.Send(
                It.Is<AlterarAvisoDoMuralCommand>(cmd => cmd.Id == avisoId && cmd.Mensagem == mensagem),
                default(CancellationToken)
            ), Times.Once);
        }
    }
}
