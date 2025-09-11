using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.ObterQuantidadeCriancaUseCase
{
    public class ObterQuantidadeCriancaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SME.SGP.Aplicacao.ObterQuantidadeCriancaUseCase useCase;

        public ObterQuantidadeCriancaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SME.SGP.Aplicacao.ObterQuantidadeCriancaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Quantidade_Quando_Encontrada()
        {
            var esperado = new QuantidadeCriancaDto { MensagemQuantidade = "Total: 100 crianças" };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterQuantidadeCriancaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(2025, new[] { "TURMA1" }, "DRE01", "UE01", new[] { 1 }, new[] { "1A" });

            Assert.NotNull(resultado);
            Assert.Equal("Total: 100 crianças", resultado.MensagemQuantidade);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Quantidade_For_Nula()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterQuantidadeCriancaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuantidadeCriancaDto)null);

            var resultado = await useCase.Executar(2025, new[] { "TURMA1" }, "DRE01", "UE01", new[] { 1 }, new[] { "1A" });

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Mediator_Falhar()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterQuantidadeCriancaQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro interno"));

            var ex = await Assert.ThrowsAsync<Exception>(() => useCase.Executar(2025, new[] { "TURMA1" }, "DRE01", "UE01", new[] { 1 }, new[] { "1A" }));
            Assert.Equal("Erro interno", ex.Message);
        }
    }
}
