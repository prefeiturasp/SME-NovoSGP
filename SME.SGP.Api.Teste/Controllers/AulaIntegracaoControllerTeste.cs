using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AulaIntegracaoControllerTeste
    {
        private readonly AulaIntegracaoController _controller;
        private readonly Mock<IObterAulasPorTurmaComponenteDataUseCase> _useCaseMock = new();
        private readonly Faker _faker;

        public AulaIntegracaoControllerTeste()
        {
            _controller = new AulaIntegracaoController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter aulas por turma e componente")]
        public async Task DeveRetornarOk_QuandoExecutarConsulta()
        {
            // Arrange
            var turmaCodigo = "T123";
            long componenteCurricular = 456;
            var dataAula = DateTime.Now;
            long dataAulaTicks = dataAula.Ticks;

            var aulasFakes = new List<AulaQuantidadeTipoDto>();
            for (int i = 1; i <= 2; i++)
            {
                aulasFakes.Add(new AulaQuantidadeTipoDto
                {
                    Id = i,
                    Quantidade = _faker.Random.Int(1, 10),
                    Tipo = _faker.Random.Int(1, 3),
                    EhCj = _faker.Random.Bool()
                });
            }

            _useCaseMock
                .Setup(x => x.Executar(It.Is<FiltroObterAulasPorTurmaComponenteDataDto>(f =>
                    f.TurmaCodigo == turmaCodigo &&
                    f.ComponenteCurricular == componenteCurricular &&
                    f.DataAula == new DateTime(dataAulaTicks)
                )))
                .ReturnsAsync(aulasFakes);

            // Act
            var resultado = await _controller.ObterAulasPorTurmaComponenteData(turmaCodigo, componenteCurricular, dataAulaTicks, _useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsAssignableFrom<IEnumerable<AulaQuantidadeTipoDto>>(okResult.Value);
            Assert.Equal(2, ((List<AulaQuantidadeTipoDto>)lista).Count);
        }
    }
}
