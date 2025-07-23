using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class CalendarioIntegracoesControllerTeste
    {
        private readonly CalendarioIntegracoesController _controller;
        private readonly Mock<IObterDiasLetivosPorUeETurnoUseCase> _obterDiasLetivosPorUeETurnoUseCase = new();
        private readonly Faker _faker;

        public CalendarioIntegracoesControllerTeste()
        {
            _controller = new CalendarioIntegracoesController();
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task ObterDiasLetivos_DeveRetornarOkComDados()
        {
            // Arrange
            var filtro = new FiltroDiasLetivosPorUeEDataDTO
            {
                UeCodigo = _faker.Random.String2(4),
                TipoTurno = _faker.Random.Int(),
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(30),
            };

            var diasLetivosEsperados = new List<DiaLetivoSimplesDto>{ };
            _obterDiasLetivosPorUeETurnoUseCase.Setup(x => x.Executar(filtro)).ReturnsAsync(diasLetivosEsperados);

            // Act
            var resultado = await _controller.ObterDiasLetivos(filtro, _obterDiasLetivosPorUeETurnoUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dadosRetornados = Assert.IsAssignableFrom<IEnumerable<DiaLetivoSimplesDto>>(okResult.Value);

            Assert.Equal(diasLetivosEsperados, dadosRetornados);
        }

    }
}
