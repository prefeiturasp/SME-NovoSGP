using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class AcompanhamentoTurmaControllerTeste
    {
        private readonly AcompanhamentoTurmaController _controller;
        private readonly Faker _faker;

        public AcompanhamentoTurmaControllerTeste()
        {
            _controller = new AcompanhamentoTurmaController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para salvar o acompanhamento da turma")]
        public async Task DeveChamarUseCase_ParaSalvarAcompanhamentoTurma()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarAcompanhamentoTurmaUseCase>();
            var dto = new AcompanhamentoTurmaDto();
            var retorno = new AcompanhamentoTurma();

            useCaseMock.Setup(u => u.Executar(dto)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.Salvar(useCaseMock.Object, dto);

            // Assert
            useCaseMock.Verify(u => u.Executar(dto), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter o apanhado geral")]
        public async Task DeveChamarUseCase_ParaObterApanhadoGeral()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAcompanhamentoTurmaApanhadoGeralUseCase>();
            var filtro = new FiltroAcompanhamentoTurmaApanhadoGeral();
            var retorno = new AcompanhamentoTurmaDto();

            useCaseMock.Setup(u => u.Executar(filtro)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.Obter(filtro, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter o parâmetro de quantidade de imagens")]
        public async Task DeveChamarUseCase_ParaObterParametroQuantidadeImagens()
        {
            // Arrange
            var useCaseMock = new Mock<IObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase>();
            var ano = _faker.Random.Int(2020, 2025);
            var retorno = new ParametroQuantidadeUploadImagemDto();

            useCaseMock.Setup(u => u.Executar(ano)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterParametroQuantidadeImagens(ano, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(ano), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }
    }
}