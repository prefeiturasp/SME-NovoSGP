using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class CicloControllerTeste
    {
        private readonly CicloController _controller;
        private readonly Mock<IConsultasCiclo> _consultasCiclo = new();

        public CicloControllerTeste()
        {
            _controller = new CicloController(_consultasCiclo.Object);
        }

        [Fact]
        public void Filtrar_DeveRetornarOkComListaDeCiclos()
        {
            // Arrange
            var filtro = new FiltroCicloDto { };

            var resultadoEsperado = new List<CicloDto>
            {
                new CicloDto { Id = 1 },
                new CicloDto { Id = 2 }
            };

            _consultasCiclo.Setup(x => x.Listar(filtro)).Returns(resultadoEsperado);

            // Act
            var resultado = _controller.Filtrar(filtro) as OkObjectResult;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
        }
    }
}
