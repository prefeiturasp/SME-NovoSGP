using Bogus;
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
    public class AtividadeInfantilControllerTeste
    {
        private readonly AtividadeInfantilController _controller;
        private readonly Faker _faker;

        public AtividadeInfantilControllerTeste()
        {
            _controller = new AtividadeInfantilController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso da lista de atividades infantis")]
        public async Task DeveRetornarLista_AtividadeInfantil()
        {
            // Arrange
            var aulaId = _faker.Random.Long(1);
            var atividadesEsperadas = new List<AtividadeInfantilDto>
            {
                new AtividadeInfantilDto { Id = _faker.Random.Int(), Titulo = _faker.Lorem.Sentence() },
                new AtividadeInfantilDto { Id = _faker.Random.Int(), Titulo = _faker.Lorem.Sentence() }
            };

            var mockUseCase = new Mock<IObterAtividadesInfantilUseCase>();
            mockUseCase.Setup(x => x.Executar(aulaId)).ReturnsAsync(atividadesEsperadas);

            // Act
            var resultado = await _controller.ObterAtividadesMural(aulaId, mockUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var atividadesRetornadas = Assert.IsAssignableFrom<IEnumerable<AtividadeInfantilDto>>(okResult.Value);
            Assert.Equal(atividadesEsperadas.Count, ((List<AtividadeInfantilDto>)atividadesRetornadas).Count);
        }
    }
}
