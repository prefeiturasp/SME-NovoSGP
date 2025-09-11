using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AnoLetivoControllerTeste
    {
        private readonly AnoLetivoController _controller;
        private readonly Faker _faker;

        public AnoLetivoControllerTeste()
        {
            _controller = new AnoLetivoController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve retornar a lista de anos letivos e atribuições")]
        public async Task DeveRetornarLista_AnosLetivosAtribuicoes()
        {
            // Arrange
            var consideraHistorico = true;
            var anosEsperados = new List<int> { 2022, 2023, 2024 };

            var mockConsultasAtribuicoes = new Mock<IConsultasAtribuicoes>();
            mockConsultasAtribuicoes
                .Setup(x => x.ObterAnosLetivos(consideraHistorico))
                .ReturnsAsync(anosEsperados);

            // Act
            var resultado = await _controller.ObterAnosLetivosAtribuicoes(consideraHistorico, mockConsultasAtribuicoes.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var anosRetornados = Assert.IsAssignableFrom<IEnumerable<int>>(okResult.Value);
            Assert.Equal(anosEsperados, anosRetornados);
        }
    }
}
