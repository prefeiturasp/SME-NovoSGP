using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task Deve_Retornar_Dados_Filtrando_Por_Ano()
        {
            //// Arrange


            //mediator.Setup(a => a.Send(It.IsAny<ObterDatasAulasPorProfessorEComponenteQuery>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(new List<DatasAulasDto>());

            //// Act
            //var datasAulas = await useCase.Executar(new ConsultaDatasAulasDto("123", "1105"));

            //// Assert
            //Assert.NotNull(datasAulas);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Filtrando_Por_Mes()
        {

        }
    }
}
