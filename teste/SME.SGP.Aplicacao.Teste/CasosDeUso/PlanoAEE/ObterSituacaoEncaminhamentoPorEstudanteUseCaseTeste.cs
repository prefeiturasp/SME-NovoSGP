using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterSituacaoEncaminhamentoPorEstudanteUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterSituacaoEncaminhamentoPorEstudanteUseCase useCase;

        public ObterSituacaoEncaminhamentoPorEstudanteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterSituacaoEncaminhamentoPorEstudanteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Situacao_Encaminhamento()
        {
            // Arrange
            var filtro = new FiltroEncaminhamentoAeeDto
            {
                EstudanteCodigo = "123",
                UeCodigo = "3"
            };

            var situacaoEsperada = new SituacaoEncaminhamentoPorEstudanteDto
            {
                Situacao = Dominio.Enumerados.SituacaoAEE.Analise.ToString(),
                Id = 3
            };

            mediator.Setup(x => x.Send(
                It.Is<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery>(
                    q => q.EstudanteCodigo == filtro.EstudanteCodigo &&
                         q.UeCodigo == filtro.UeCodigo),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(situacaoEsperada);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Analise", resultado.Situacao);
            Assert.Equal(Dominio.Enumerados.SituacaoAEE.Analise.ToString(), resultado.Situacao);
        }

        [Fact]
        public async Task Executar_Com_Filtro_Nulo_Deve_Lancar_Excecao()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SituacaoEncaminhamentoPorEstudanteDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<System.NullReferenceException>(() => useCase.Executar(null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            // Arrange
            var filtro = new FiltroEncaminhamentoAeeDto
            {
                EstudanteCodigo = "456",
                UeCodigo = "2"
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SituacaoEncaminhamentoPorEstudanteDto());

            // Act
            await useCase.Executar(filtro);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery>(
                    q => q.EstudanteCodigo == filtro.EstudanteCodigo &&
                         q.UeCodigo == filtro.UeCodigo),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Lanca_Excecao_Deve_Repassar()
        {
            // Arrange
            var filtro = new FiltroEncaminhamentoAeeDto
            {
                EstudanteCodigo = "789",
                UeCodigo = "3"
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterSituacaoEncaminhamentoAEEPorEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro simulado"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => useCase.Executar(filtro));
        }
    }
}
