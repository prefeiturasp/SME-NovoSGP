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
    public class AtualizarInformacoesDoPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly AtualizarInformacoesDoPlanoAEEUseCase useCase;

        public AtualizarInformacoesDoPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new AtualizarInformacoesDoPlanoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_E_Disparar_Comandos_Para_Todos_Os_Planos()
        {
            // Arrange
            var planos = new List<PlanoAEETurmaDto>
            {
                new PlanoAEETurmaDto { Id = 1 },
                new PlanoAEETurmaDto { Id = 2 }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planos);

            mediator.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                c => c.Rota == RotasRabbitSgp.ExecutarAtualizacaoDaTurmaDoPlanoAEE),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Nao_Existirem_Planos()
        {
            // Arrange
            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEETurmaDto>());

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Ao_Obter_Planos()
        {
            // Arrange
            mediator.Setup(m => m.Send(It.IsAny<ObterPlanosComSituacaoDiferenteDeEncerradoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter planos"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(new MensagemRabbit()));
        }
    }
}