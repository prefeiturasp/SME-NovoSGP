using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashBoardControllerTeste
    {
        private readonly DashBoardController _controller;
        private readonly Mock<IObterDashBoardUseCase> _obterDashboardUseCase;

        public DashBoardControllerTeste()
        {
            _obterDashboardUseCase = new Mock<IObterDashBoardUseCase>();
            _controller = new DashBoardController();
        }

        [Fact(DisplayName = "Obter deve retornar Ok com lista de Dashboard")]
        public async Task Obter_DeveRetornarOk_ComLista()
        {
            // Arrange
            var lista = new List<DashBoard>
            {
                new DashBoard()
            };

            _obterDashboardUseCase
                .Setup(s => s.Executar())
                .ReturnsAsync(lista);

            // Act
            var result = await _controller.Obter(_obterDashboardUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<DashBoard>>(ok.Value);
        }
    }
}
