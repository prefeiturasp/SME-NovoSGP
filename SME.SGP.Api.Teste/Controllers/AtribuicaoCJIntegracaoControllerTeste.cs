using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AtribuicaoCJIntegracaoControllerTeste
    {
        private readonly AtribuicaoCJIntegracaoController _controller;
        private readonly Mock<IListarAtribuicoesCJPorFiltroUseCase> _useCaseMock;
        private readonly Faker _faker;

        public AtribuicaoCJIntegracaoControllerTeste()
        {
            _useCaseMock = new Mock<IListarAtribuicoesCJPorFiltroUseCase>();
            _controller = new AtribuicaoCJIntegracaoController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso com lista de atribuições CJ")]
        public async Task ObterAtribuicoesCJ_DeveRetornarSucesso_QuandoHouverDados()
        {
            // Arrange
            var ueCodigo = _faker.Random.AlphaNumeric(6);
            var anoLetivo = _faker.Random.Int(2020, 2030);

            var retornoEsperado = new List<AtribuicaoCJListaRetornoDto>
            {
                new AtribuicaoCJListaRetornoDto
                {
                    Disciplinas = new[] { "Matemática", "Português" },
                    DisciplinasId = new long[] { 1, 2 },
                    Modalidade = "Fundamental",
                    ModalidadeId = 1,
                    Turma = "5ºA",
                    TurmaId = _faker.Random.Guid().ToString(),
                    ProfessorRf = _faker.Random.AlphaNumeric(6)
                }
            };

            _useCaseMock
                .Setup(x => x.Executar(It.IsAny<AtribuicaoCJListaFiltroDto>()))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.Get(ueCodigo, anoLetivo, _useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<AtribuicaoCJListaRetornoDto>>(okResult.Value);
            Assert.Single(retorno);
        }

        [Fact(DisplayName = "Deve retornar 204 NoContent quando não houver atribuições CJ")]
        public async Task ObterAtribuicoesCJ_DeveRetornarNoContent_QuandoNaoHouverDados()
        {
            // Arrange
            var ueCodigo = _faker.Random.AlphaNumeric(6);
            var anoLetivo = _faker.Random.Int(2020, 2030);

            _useCaseMock
                .Setup(x => x.Executar(It.IsAny<AtribuicaoCJListaFiltroDto>()))
                .ReturnsAsync(new List<AtribuicaoCJListaRetornoDto>());

            // Act
            var resultado = await _controller.Get(ueCodigo, anoLetivo, _useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }
    }
}
