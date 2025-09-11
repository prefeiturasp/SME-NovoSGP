using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class CacheControllerTeste
    {
        private readonly CacheController _controller;
        private readonly Mock<IObterPrefixosCacheUseCase> _obterPrefixosCacheUseCase = new();
        private readonly Faker _faker;

        public CacheControllerTeste()
        {
            _controller = new CacheController();
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void ObterChaves_DeveRetornarListaDeStrings()
        {
            // Act
            var resultado = _controller.ObterChaves();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaChaves = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.NotEmpty(listaChaves); // Se esperar que tenha algum valor
        }

        [Fact]
        public void ObterPrefixos_DeveRetornarListaDoUseCase()
        {
            // Arrange
            var prefixosEsperados = new List<string> { "prefixo1", "prefixo2" };
            _obterPrefixosCacheUseCase.Setup(u => u.Executar()).Returns(prefixosEsperados);

            // Act
            var resultado = _controller.ObterPrefixos(_obterPrefixosCacheUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaPrefixos = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Equal(prefixosEsperados, listaPrefixos);
        }
    }
}
