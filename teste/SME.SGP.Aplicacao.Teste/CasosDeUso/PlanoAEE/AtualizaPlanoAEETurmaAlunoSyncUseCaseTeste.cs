using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class AtualizaPlanoAEETurmaAlunoSyncUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly AtualizaPlanoAEETurmaAlunoSyncUseCase useCase;

        public AtualizaPlanoAEETurmaAlunoSyncUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new AtualizaPlanoAEETurmaAlunoSyncUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_E_Nao_Publicar_Fila_Quando_Nao_Encontrar_Planos()
        {
            // Arrange
            var mensagem = new MensagemRabbit();
            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEETurmaDto>());

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_True_E_Publicar_Fila_Para_Cada_Plano()
        {
            // Arrange
            var mensagem = new MensagemRabbit();
            var planos = new List<PlanoAEETurmaDto>
            {
                new PlanoAEETurmaDto { Id = 1, AlunoCodigo = "123" },
                new PlanoAEETurmaDto { Id = 2, AlunoCodigo = "456" }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planos);

            mediator.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Rota == RotasRabbitSgpAEE.AtualizarTabelaPlanoAEETurmaAlunoTratar),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Do_Mediator_ObterPlanos()
        {
            // Arrange
            var mensagem = new MensagemRabbit();
            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter planos"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Do_Mediator_PublicarFila()
        {
            // Arrange
            var mensagem = new MensagemRabbit();
            var planos = new List<PlanoAEETurmaDto> { new PlanoAEETurmaDto { Id = 1, AlunoCodigo = "123" } };

            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planos);

            mediator.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao publicar fila"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Deve_Falhar_Se_Qualquer_Plano_Falhar()
        {
            // Arrange
            var mensagem = new MensagemRabbit();
            var planos = new List<PlanoAEETurmaDto>
             {
               new PlanoAEETurmaDto { Id = 1, AlunoCodigo = "123" },
               new PlanoAEETurmaDto { Id = 2, AlunoCodigo = "456" }
             };

            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planos);

            mediator.SetupSequence(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ThrowsAsync(new Exception("Erro no segundo plano"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagem));
            mediator.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}