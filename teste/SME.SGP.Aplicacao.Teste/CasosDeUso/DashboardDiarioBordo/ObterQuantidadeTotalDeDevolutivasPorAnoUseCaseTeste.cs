using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterQuantidadeTotalDeDevolutivasPorAnoUseCaseTeste
    {
        private readonly ObterQuantidadeTotalDeDevolutivasPorDREUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterQuantidadeTotalDeDevolutivasPorAnoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterQuantidadeTotalDeDevolutivasPorDREUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Filtrando_Por_Mes()
        {
            // Arrange
            var grafico = new List<GraficoTotalDevolutivasPorAnoDTO>();
            var dados = new GraficoTotalDevolutivasPorAnoDTO() { Ano = "1", Descricao = "Devolutivas", Quantidade = 10 };
            grafico.Add(dados);
            
            mediator.Setup(a => a.Send(It.IsAny<ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grafico);

            // Act
            var dadosGrafico = await useCase.Executar(new FiltroDasboardDiarioBordoDevolutivasDto() { AnoLetivo = 2022, DreId = 0, Mes = 5 });
            mediator.Verify(x => x.Send(It.IsAny<ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            // Assert
            Assert.NotNull(dadosGrafico);
        }
    }
}
