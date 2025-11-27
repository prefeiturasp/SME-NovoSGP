using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ComunicadoEventoControllerTeste
    {
        private readonly ComunicadoEventoController _controller;
        private readonly Mock<IListarEventosPorCalendarioUseCase> _listarEventosPorCalendarioUseCase = new();

        public ComunicadoEventoControllerTeste()
        {
            _controller = new ComunicadoEventoController();
        }

        [Fact(DisplayName = "ListarEventosPorCalendario deve retornar Ok quando houver eventos")]
        public async Task ListarEventosPorCalendario_DeveRetornarOk_QuandoHouverEventos()
        {
            // Arrange
            var filtro = new ListarEventoPorCalendarioDto();

            var retorno = new List<EventoCalendarioRetornoDto>
            {
                new EventoCalendarioRetornoDto()
            };

            _listarEventosPorCalendarioUseCase
                .Setup(x => x.Executar(filtro))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ListarEventosPorCalendario(
                filtro,
                _listarEventosPorCalendarioUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<List<EventoCalendarioRetornoDto>>(ok.Value);
            Assert.Single(valor);
        }

    }
}
