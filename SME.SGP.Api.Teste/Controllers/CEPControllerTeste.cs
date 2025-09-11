using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class CEPControllerTeste
    {
        private readonly CEPController _controller;
        private readonly Mock<IBuscaCepUseCase> _buscaCepUseCase = new();

        public CEPControllerTeste()
        {
            _controller = new CEPController();
        }

        [Fact]
        public async Task BuscarCep_DeveRetornarOk_QuandoCepEncontrado()
        {
            // Arrange
            var cep = "12345678";
            var retornoEsperado = new CepDto
            {
                Cep = cep,
                Logradouro = "Rua Exemplo",
                Bairro = "Centro",
                Localidade = "São Paulo",
                UF = "SP"
            };

            _buscaCepUseCase.Setup(x => x.Executar(cep)).ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.BuscarCep(cep, _buscaCepUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<CepDto>(okResult.Value);
            Assert.Equal(retornoEsperado.Cep, retorno.Cep);
            Assert.Equal(retornoEsperado.Logradouro, retorno.Logradouro);
        }

        [Fact]
        public async Task BuscarCep_DeveRetornarNoContent_QuandoRetornoForNulo()
        {
            // Arrange
            var cep = "99999999";
            _buscaCepUseCase.Setup(x => x.Executar(cep)).ReturnsAsync((CepDto)null); 

            // Act
            var resultado = await _controller.BuscarCep(cep, _buscaCepUseCase.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }
    }
}
