using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class CompensacaoAusenciaControllerTeste
    {
        private readonly CompensacaoAusenciaController _controller;
        private readonly Mock<IPeriodoDeCompensacaoAbertoUseCase> _periodoDeCompensacaoAbertoUseCase = new();
        private readonly Mock<IConsultasCompensacaoAusencia> _consultasCompensacaoAusencia = new();
        private readonly Mock<ISalvarCompensacaoAusenciaUseCase> _salvarCompensacaoAusenciaUseCase = new();
        private readonly Mock<IExcluirCompensacaoAusenciaUseCase> _excluirCompensacaoAusenciaUseCase = new();
        private readonly Mock<ICopiarCompensacaoAusenciaUseCase> _copiarCompensacaoAusenciaUseCase = new();
        private readonly Faker _faker;

        public CompensacaoAusenciaControllerTeste()
        {
            _controller = new CompensacaoAusenciaController();
            _faker = new Faker("pt_BR");
        }

        #region VerificarPeriodoAberto
        [Fact]
        public async Task VerificarPeriodoAberto_DeveRetornarOkComResultadoEsperado()
        {
            // Arrange
            var turmaCodigo = "123";
            var bimestre = 2;

            _periodoDeCompensacaoAbertoUseCase.Setup(x => x.VerificarPeriodoAberto(turmaCodigo, bimestre)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.VerificarPeriodoAberto(turmaCodigo, bimestre, _periodoDeCompensacaoAbertoUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            Assert.IsType<bool>(okResult.Value);
            Assert.True((bool)okResult.Value);
        }
        #endregion

        #region ListarCompensacaoAusencias
        [Fact]
        public async Task ListarCompensacaoAusencias_DeveRetornarOkComPaginacao()
        {
            // Arrange
            var filtros = new FiltroCompensacoesAusenciaDto
            {
                TurmaId = _faker.Random.String2(2),
                DisciplinaId = _faker.Random.String2(2),
                Bimestre = _faker.Random.Int(1, 10),
                AtividadeNome = _faker.Random.String2(10),
                AlunoNome = _faker.Name.FullName()
            };

            var resultadoEsperado = new PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>
            {
                TotalRegistros = 1,
                Items = new List<CompensacaoAusenciaListagemDto>
        {
            new CompensacaoAusenciaListagemDto { Id = 1, AtividadeNome = _faker.Lorem.Sentences() }
        }
            };

            _consultasCompensacaoAusencia
                .Setup(x => x.ListarPaginado(filtros.TurmaId, filtros.DisciplinaId, filtros.Bimestre, filtros.AtividadeNome, filtros.AlunoNome))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.listar(filtros, _consultasCompensacaoAusencia.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            var retorno = Assert.IsType<PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>>(okResult.Value);
            Assert.Single(retorno.Items);
        }
        #endregion

        #region ObterCompensacaoAusenciaPorID
        [Fact]
        public async Task ObterCompensacaoAusenciaPorID_DeveRetornarOkComCompensacao()
        {
            // Arrange
            long id = 42;

            var dto = new CompensacaoAusenciaCompletoDto { Id = id };

            _consultasCompensacaoAusencia.Setup(x => x.ObterPorId(id)).ReturnsAsync(dto);

            // Act
            var resultado = await _controller.Obter(id, _consultasCompensacaoAusencia.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            var retorno = Assert.IsType<CompensacaoAusenciaCompletoDto>(okResult.Value);
            Assert.Equal(id, retorno.Id);
        }
        #endregion

        #region IncluirCompensacaoAusencia
        [Fact]
        public async Task IncluirCompensacaoAusencia_DeveRetornarOk()
        {
            // Arrange
            var dto = new CompensacaoAusenciaDto { };

            _salvarCompensacaoAusenciaUseCase.Setup(x => x.Executar(0, dto)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.Inserir(dto, _salvarCompensacaoAusenciaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region AlterarCompensacaoAusencia
        [Fact]
        public async Task Alterar_DeveRetornarOk()
        {
            // Arrange
            long id = 10;
            var dto = new CompensacaoAusenciaDto { };

            _salvarCompensacaoAusenciaUseCase.Setup(x => x.Executar(id, dto)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.Alterar(id, dto, _salvarCompensacaoAusenciaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region ExcluirCompensacaoAusencia
        [Fact]
        public async Task Excluir_DeveRetornarOk()
        {
            // Arrange
            var ids = new long[] { 1, 2, 3 };

            _excluirCompensacaoAusenciaUseCase
                .Setup(x => x.Executar(ids))
                .ReturnsAsync(true);

            // Act
            var resultado = await _controller.Excluir(ids, _excluirCompensacaoAusenciaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region ObterTurmasCopia
        [Fact]
        public async Task ObterTurmasCopia_DeveRetornarOkComTurmas()
        {
            // Arrange
            string turmaOrigemCodigo = "ABC123";

            var turmas = new List<TurmaRetornoDto>
            {
                new TurmaRetornoDto { Codigo = "T1" },
                new TurmaRetornoDto { Codigo = "T2" }
            };

            _consultasCompensacaoAusencia.Setup(x => x.ObterTurmasParaCopia(turmaOrigemCodigo)).ReturnsAsync(turmas);

            // Act
            var resultado = await _controller.ObterTurmasCopia(turmaOrigemCodigo, _consultasCompensacaoAusencia.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            var retorno = Assert.IsType<List<TurmaRetornoDto>>(okResult.Value);
            Assert.Equal(2, retorno.Count);
        }
        #endregion

        #region Copiar
        [Fact]
        public async Task Copiar_DeveRetornarOk()
        {
            // Arrange
            var dto = new CompensacaoAusenciaCopiaDto { };
            var retornoEsperado = "Cópia realizada com sucesso";

            _copiarCompensacaoAusenciaUseCase
                .Setup(x => x.Executar(dto))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.Copiar(dto, _copiarCompensacaoAusenciaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            Assert.Equal(retornoEsperado, okResult.Value);
        }
        #endregion
    }
}
