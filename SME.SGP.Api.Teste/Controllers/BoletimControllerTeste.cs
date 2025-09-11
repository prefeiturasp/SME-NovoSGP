using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class BoletimControllerTeste
    {
        private readonly BoletimController _controller;
        private readonly Mock<IBoletimUseCase> _boletimUseCase = new();
        private readonly Mock<IObterListaAlunosDaTurmaUseCase> _obterListaAlunosDaTurmaUseCase = new();
        private readonly Faker _faker;

        public BoletimControllerTeste()
        {
            _controller = new BoletimController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Imprimir deve retornar Ok com resultado do caso de uso")]
        public async Task Imprimir_DeveRetornarOkComResultado()
        {
            // Arrange
            var filtroDto = new FiltroRelatorioBoletimDto
            {
                DreCodigo = _faker.Random.AlphaNumeric(6)
            };

            bool resultadoEsperado = true;

            _boletimUseCase
                .Setup(u => u.Executar(It.IsAny<FiltroRelatorioBoletimDto>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.Imprimir(filtroDto, _boletimUseCase.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact(DisplayName = "ListarAlunos deve retornar Ok com lista de alunos")]
        public async Task ListarAlunos_DeveRetornarOkComLista()
        {
            // Arrange
            var turmaCodigo = _faker.Random.String2(6);

            var listaEsperada = new PaginacaoResultadoDto<AlunoSimplesDto>
            {
                Items = new[]
                {
                    new AlunoSimplesDto { Codigo= _faker.Random.AlphaNumeric(6), Nome = _faker.Name.FullName() }
                },
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            _obterListaAlunosDaTurmaUseCase
                .Setup(u => u.Executar(turmaCodigo))
                .ReturnsAsync(listaEsperada);

            // Act
            var resultado = await _controller.ListarAlunos(turmaCodigo, _obterListaAlunosDaTurmaUseCase.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(listaEsperada, okResult.Value);
        }

        [Fact(DisplayName = "ListarAlunos (com observações) deve retornar Ok com lista de alunos com observações")]
        public async Task ListarAlunosObservacoes_DeveRetornarOkComLista()
        {
            // Arrange
            var turmaCodigo = _faker.Random.String2(6);

            var listaEsperada = new PaginacaoResultadoDto<AlunoSimplesDto>
            {
                Items = new[]
                {
            new AlunoSimplesDto { Codigo= _faker.Random.AlphaNumeric(6), Nome = _faker.Name.FullName() }
        },
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            _obterListaAlunosDaTurmaUseCase
                .Setup(u => u.Executar(turmaCodigo))
                .ReturnsAsync(listaEsperada);

            // Act
            var resultado = await _controller.ListarAlunos(turmaCodigo, _obterListaAlunosDaTurmaUseCase.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(listaEsperada, okResult.Value);
        }
    }
}
