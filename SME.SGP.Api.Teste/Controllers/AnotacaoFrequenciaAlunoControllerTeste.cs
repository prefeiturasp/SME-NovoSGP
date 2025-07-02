using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AnotacaoFrequenciaAlunoControllerTeste
    {
        [Fact]
        public async Task ObterJustificativaCompleto_DeveRetornarOkResult_SeEncontrar()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAnotacaoFrequenciaAlunoPorIdUseCase>();
            var id = 1;
            var resultadoEsperado = new AnotacaoFrequenciaAlunoCompletoDto();

            useCaseMock
                .Setup(x => x.Executar(id))
                .ReturnsAsync(resultadoEsperado);

            var controller = new AnotacaoFrequenciaAlunoController();

            // Act
            var resultado = await controller.ObterJustificativaCompleto(id, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarNoContent_SeNaoEncontrar()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAnotacaoFrequenciaAlunoUseCase>();
            var codigoAluno = "123";
            var aulaId = 10L;

            useCaseMock
                .Setup(x => x.Executar(It.IsAny<FiltroAnotacaoFrequenciaAlunoDto>()))
                .ReturnsAsync((AnotacaoFrequenciaAlunoDto)null);

            var controller = new AnotacaoFrequenciaAlunoController();

            // Act
            var resultado = await controller.BuscarPorId(codigoAluno, aulaId, useCaseMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public async Task ObterPorAlunoPorPeriodo_DeveRetornarOkResult_SeEncontrar()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAnotacaoFrequenciaAlunoPorPeriodoUseCase>();
            var codigoAluno = "123456";
            var dataInicio = new DateTime(2025, 7, 1);
            var dataFim = new DateTime(2025, 7, 2);

            var resultadoEsperado = new List<AnotacaoAlunoAulaPorPeriodoDto>
            {
                new AnotacaoAlunoAulaPorPeriodoDto()
            };

            useCaseMock
                .Setup(x => x.Executar(It.Is<FiltroAnotacaoFrequenciaAlunoPorPeriodoDto>(f =>
                    f.CodigoAluno == codigoAluno &&
                    f.DataInicio == dataInicio &&
                    f.DataFim == dataFim)))
                .ReturnsAsync(resultadoEsperado);

            var controller = new AnotacaoFrequenciaAlunoController();

            // Act
            var resultado = await controller.ObterPorAlunoPorPeriodo(codigoAluno, dataInicio, dataFim, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }


        [Fact]
        public async Task Salvar_DeveRetornarOkResult_QuandoSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarAnotacaoFrequenciaAlunoUseCase>();
            var dto = new SalvarAnotacaoFrequenciaAlunoDto();
            var resultadoEsperado = new AuditoriaDto();

            useCaseMock
                .Setup(x => x.Executar(dto))
                .ReturnsAsync(resultadoEsperado);

            var controller = new AnotacaoFrequenciaAlunoController();

            // Act
            var resultado = await controller.Salvar(dto, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Alterar_DeveRetornarOkResult_QuandoSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<IAlterarAnotacaoFrequenciaAlunoUseCase>();
            var id = 1L;

            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = 2,
                Anotacao = "Teste anotação"
            };

            useCaseMock
                .Setup(x => x.Executar(It.Is<AlterarAnotacaoFrequenciaAlunoDto>(x => x.Id == id)))
                .ReturnsAsync(true);

            var controller = new AnotacaoFrequenciaAlunoController();

            // Act
            var resultado = await controller.Alterar(id, dto, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True((bool)okResult.Value);
        }


        [Fact]
        public async Task ListarMotivos_DeveRetornarOkResult()
        {
            // Arrange
            var useCaseMock = new Mock<IObterMotivosAusenciaUseCase>();
            var resultadoEsperado = new List<OpcaoDropdownDto> { new OpcaoDropdownDto() };

            useCaseMock
                .Setup(x => x.Executar())
                .ReturnsAsync(resultadoEsperado);

            var controller = new AnotacaoFrequenciaAlunoController();

            // Act
            var resultado = await controller.ListarMotivos(useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Excluir_DeveRetornarOkResult()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirAnotacaoFrequenciaAlunoUseCase>();
            var id = 1L;
            var resultadoEsperado = new RetornoBaseDto();

            useCaseMock
                .Setup(x => x.Executar(id))
                .ReturnsAsync(true);

            var controller = new AnotacaoFrequenciaAlunoController();

            // Act
            var resultado = await controller.Excluir(id, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True((bool)okResult.Value);
        }

    }
}
