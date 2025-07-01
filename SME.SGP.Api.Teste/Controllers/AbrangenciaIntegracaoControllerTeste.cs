using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class AbrangenciaIntegracaoControllerTeste
    {
        private readonly AbrangenciaIntegracaoController _controller;
        private readonly Faker _faker;

        public AbrangenciaIntegracaoControllerTeste()
        {
            _controller = new AbrangenciaIntegracaoController();
            _faker = new Faker("pt_BR");
        }

        [Theory(DisplayName = "Deve chamar o caso de uso para verificar acesso à sondagem e retornar o resultado")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PodeAcessarSondagem_DeveChamarCasoDeUso_E_RetornarResultado(bool resultadoEsperado)
        {
            // Arrange
            var useCaseMock = new Mock<IUsuarioPossuiAbrangenciaAcessoSondagemUseCase>();
            var usuarioRF = _faker.Random.AlphaNumeric(7);
            var usuarioPerfil = Guid.NewGuid();

            useCaseMock.Setup(u => u.Executar(usuarioRF, usuarioPerfil))
                       .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.PodeAcessarSondagem(usuarioRF, usuarioPerfil, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(usuarioRF, usuarioPerfil), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<bool>(okResult.Value);

            Assert.Equal(resultadoEsperado, valorRetornado);
        }
    }
}