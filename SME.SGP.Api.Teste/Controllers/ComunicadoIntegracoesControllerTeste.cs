using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ComunicadoIntegracoesControllerTeste
    {
        private readonly Mock<IObterComunicadosAnoAtualUseCase> _obterComunicadosAnoAtualUseCase = new ();
        private readonly ComunicadoIntegracoesController _controller;

        public ComunicadoIntegracoesControllerTeste()
        {
            _controller = new ComunicadoIntegracoesController();
        }

        [Fact(DisplayName = "ObterTodosAnoAtual deve retornar Ok com lista de comunicados")]
        public async Task ObterTodosAnoAtual_DeveRetornarOk()
        {
            // Arrange
            var comunicados = new List<ComunicadoTurmaAlunoDto>
            {
                new ComunicadoTurmaAlunoDto()
            };

            _obterComunicadosAnoAtualUseCase
                .Setup(x => x.Executar())
                .ReturnsAsync(comunicados);

            // Act
            var resultado = await _controller.ObterTodosAnoAtual(
                _obterComunicadosAnoAtualUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<List<ComunicadoTurmaAlunoDto>>(ok.Value);
            Assert.Single(valorRetornado);
        }

    }
}
