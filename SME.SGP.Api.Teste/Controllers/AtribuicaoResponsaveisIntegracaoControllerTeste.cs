using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AtribuicaoResponsaveisIntegracaoControllerTeste
    {
        private readonly AtribuicaoResponsaveisIntegracaoController _controller;
        private readonly Mock<IListarAtribuicoesResponsaveisPorFiltroUseCase> _useCaseMock = new();
        private readonly Faker _faker;

        public AtribuicaoResponsaveisIntegracaoControllerTeste()
        {
            _controller = new AtribuicaoResponsaveisIntegracaoController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso que retorna atribuições responsaveis")]
        public async Task DeveRetornarOk_QuandoExistiremResponsaveis()
        {
            var ueCodigo = _faker.Random.AlphaNumeric(6);
            var tipo = (int)TipoResponsavelAtribuicao.SupervisorEscolar;

            // Arrange
            var responsaveisFakes = new List<AtribuicaoResponsavelDto>();
            for (int i = 0; i < 5; i++)
            {
                responsaveisFakes.Add(new AtribuicaoResponsavelDto
                {
                    NomeResponsavel = _faker.Name.FullName(),
                    CodigoRF = _faker.Random.Int(100000, 999999).ToString()
                });
            }

            _useCaseMock
                .Setup(x => x.Executar(It.IsAny<AtribuicaoResponsaveisFiltroDto>()))
                .ReturnsAsync(responsaveisFakes);

            // Act
            var result = await _controller.Get(ueCodigo , tipo, _useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsAssignableFrom<IEnumerable<AtribuicaoResponsavelDto>>(okResult.Value);
            Assert.Equal(5, lista.Count());
        }
    }
}
