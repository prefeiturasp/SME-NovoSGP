using Moq;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtendimentoNAAPA.AtualizarSituacaoAtendimentoNAAPA
{
    public class AtualizarSituacaoAtendimentoNAAPACommandHandlerTeste
    {
        private readonly Mock<IRepositorioAtendimentoNAAPA> _repositorioMock;
        private readonly AtualizarSituacaoAtendimentoNAAPACommandHandler _handler;

        public AtualizarSituacaoAtendimentoNAAPACommandHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioAtendimentoNAAPA>();
            _handler = new AtualizarSituacaoAtendimentoNAAPACommandHandler(_repositorioMock.Object);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_Repositorio_For_Nulo()
        {
            var excecao = Assert.Throws<ArgumentNullException>(() =>
                new AtualizarSituacaoAtendimentoNAAPACommandHandler(null));

            Assert.Equal("repositorioAtendimentoNAAPA", excecao.ParamName);
        }

        [Fact]
        public async Task Handle_Deve_Chamar_AtualizarSituacaoAtendimento_Com_Parametros_Corretos()
        {
            const long atendimentoId = 123;
            var command = new AtualizarSituacaoAtendimentoNAAPACommand(atendimentoId);
            var cancellationToken = CancellationToken.None;

            _repositorioMock
                .Setup(r => r.AtualizarSituacaoAtendimento(It.IsAny<long>(), It.IsAny<SituacaoNAAPA>()))
                .Returns(Task.CompletedTask);

            var resultado = await _handler.Handle(command, cancellationToken);

            Assert.True(resultado);
            _repositorioMock.Verify(
                r => r.AtualizarSituacaoAtendimento(atendimentoId, SituacaoNAAPA.EmApoio),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_True_Sempre_Apos_Execucao()
        {
            const long atendimentoId = 456;
            var command = new AtualizarSituacaoAtendimentoNAAPACommand(atendimentoId);
            var cancellationToken = CancellationToken.None;

            _repositorioMock
                .Setup(r => r.AtualizarSituacaoAtendimento(It.IsAny<long>(), It.IsAny<SituacaoNAAPA>()))
                .Returns(Task.CompletedTask);

            var resultado = await _handler.Handle(command, cancellationToken);

            Assert.True(resultado);
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(100L)]
        [InlineData(999999L)]
        public async Task Handle_Deve_Funcionar_Com_Diferentes_Ids_De_Atendimento(long atendimentoId)
        {
            var command = new AtualizarSituacaoAtendimentoNAAPACommand(atendimentoId);
            var cancellationToken = CancellationToken.None;

            _repositorioMock
                .Setup(r => r.AtualizarSituacaoAtendimento(It.IsAny<long>(), It.IsAny<SituacaoNAAPA>()))
                .Returns(Task.CompletedTask);

            var resultado = await _handler.Handle(command, cancellationToken);

            Assert.True(resultado);
            _repositorioMock.Verify(
                r => r.AtualizarSituacaoAtendimento(atendimentoId, SituacaoNAAPA.EmApoio),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Propagar_Excecao_Quando_Repositorio_Falhar()
        {
            const long atendimentoId = 789;
            var command = new AtualizarSituacaoAtendimentoNAAPACommand(atendimentoId);
            var cancellationToken = CancellationToken.None;
            var excecaoEsperada = new InvalidOperationException("Erro no repositório");

            _repositorioMock
                .Setup(r => r.AtualizarSituacaoAtendimento(It.IsAny<long>(), It.IsAny<SituacaoNAAPA>()))
                .ThrowsAsync(excecaoEsperada);

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, cancellationToken));

            Assert.Equal(excecaoEsperada.Message, excecao.Message);
            _repositorioMock.Verify(
                r => r.AtualizarSituacaoAtendimento(atendimentoId, SituacaoNAAPA.EmApoio),
                Times.Once);
        }
    }
}