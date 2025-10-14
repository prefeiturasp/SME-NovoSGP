using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class PeriodoFechamentoControllerTeste
    {
        private readonly Mock<IConsultasPeriodoFechamento> _consultasPeriodoFechamentoMock;
        private readonly Mock<IComandosPeriodoFechamento> _comandosPeriodoFechamentoMock;
        private readonly Mock<IPeriodoFechamentoUseCase> _periodoFechamentoUseCaseMock;
        private readonly PeriodoFechamentoController _controller;

        public PeriodoFechamentoControllerTeste()
        {
            _consultasPeriodoFechamentoMock = new Mock<IConsultasPeriodoFechamento>();
            _comandosPeriodoFechamentoMock = new Mock<IComandosPeriodoFechamento>();
            _periodoFechamentoUseCaseMock = new Mock<IPeriodoFechamentoUseCase>();
            _controller = new PeriodoFechamentoController();
        }

        [Fact]
        public async Task Get_Quando_FiltroFechamentoDto_Valido_Deve_Retornar_Ok_Com_FechamentoDto()
        {
            var filtroFechamentoDto = new FiltroFechamentoDto
            {
                TipoCalendarioId = 1,
                DreId = "123",
                UeId = "456"
            };

            var fechamentoDtoEsperado = new FechamentoDto
            {
                Id = 1,
                TipoCalendarioId = 1
            };

            _consultasPeriodoFechamentoMock
                .Setup(x => x.ObterPorTipoCalendarioSme(filtroFechamentoDto))
                .ReturnsAsync(fechamentoDtoEsperado);

            var resultado = await _controller.Get(filtroFechamentoDto, _consultasPeriodoFechamentoMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var fechamentoDto = Assert.IsType<FechamentoDto>(okResult.Value);
            Assert.Equal(fechamentoDtoEsperado.Id, fechamentoDto.Id);
            Assert.Equal(fechamentoDtoEsperado.TipoCalendarioId, fechamentoDto.TipoCalendarioId);
        }

        [Fact]
        public async Task Get_Quando_FiltroFechamentoDto_Nulo_Deve_Retornar_Ok_Com_Resultado_Null()
        {
            _consultasPeriodoFechamentoMock
                .Setup(x => x.ObterPorTipoCalendarioSme(It.IsAny<FiltroFechamentoDto>()))
                .ReturnsAsync((FechamentoDto)null);

            var resultado = await _controller.Get(null, _consultasPeriodoFechamentoMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async Task Get_Quando_ConsultasPeriodoFechamento_Lanca_Excecao_Deve_Propagar_Excecao()
        {
            var filtroFechamentoDto = new FiltroFechamentoDto();
            var excecaoEsperada = new Exception("Erro na consulta");

            _consultasPeriodoFechamentoMock
                .Setup(x => x.ObterPorTipoCalendarioSme(filtroFechamentoDto))
                .ThrowsAsync(excecaoEsperada);

            var excecao = await Assert.ThrowsAsync<Exception>(() =>
                _controller.Get(filtroFechamentoDto, _consultasPeriodoFechamentoMock.Object));

            Assert.Equal(excecaoEsperada.Message, excecao.Message);
        }

        [Fact]
        public async Task Post_Quando_FechamentoDto_Valido_Deve_Retornar_Ok()
        {
            var fechamentoDto = new FechamentoDto
            {
                TipoCalendarioId = 1
            };

            _comandosPeriodoFechamentoMock
                .Setup(x => x.Salvar(fechamentoDto))
                .Returns(Task.CompletedTask);

            var resultado = await _controller.Post(fechamentoDto, _comandosPeriodoFechamentoMock.Object);

            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            _comandosPeriodoFechamentoMock.Verify(x => x.Salvar(fechamentoDto), Times.Once);
        }

        [Fact]
        public async Task Post_Quando_FechamentoDto_Nulo_Deve_Retornar_Ok()
        {
            _comandosPeriodoFechamentoMock
                .Setup(x => x.Salvar(It.IsAny<FechamentoDto>()))
                .Returns(Task.CompletedTask);

            var resultado = await _controller.Post(null, _comandosPeriodoFechamentoMock.Object);

            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            _comandosPeriodoFechamentoMock.Verify(x => x.Salvar(null), Times.Once);
        }

        [Fact]
        public async Task Post_Quando_ComandosPeriodoFechamento_Lanca_Excecao_Deve_Propagar_Excecao()
        {
            var fechamentoDto = new FechamentoDto();
            var excecaoEsperada = new Exception("Erro ao salvar");

            _comandosPeriodoFechamentoMock
                .Setup(x => x.Salvar(fechamentoDto))
                .ThrowsAsync(excecaoEsperada);

            var excecao = await Assert.ThrowsAsync<Exception>(() =>
                _controller.Post(fechamentoDto, _comandosPeriodoFechamentoMock.Object));

            Assert.Equal(excecaoEsperada.Message, excecao.Message);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_TurmaCodigo_Valido_E_DataReferencia_Valida_Deve_Retornar_True()
        {
            var turmaCodigo = "123456";
            var dataReferencia = new DateTime(2025, 10, 8);
            var bimestre = 2;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(true);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_TurmaCodigo_Valido_E_DataReferencia_Valida_Deve_Retornar_False()
        {
            var turmaCodigo = "123456";
            var dataReferencia = new DateTime(2025, 10, 8);
            var bimestre = 1;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(false);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.False(retorno);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_DataReferencia_MinValue_Deve_Usar_DateTime_Now()
        {
            var turmaCodigo = "123456";
            var dataReferenciaMinValue = DateTime.MinValue;
            var bimestre = 3;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, It.Is<DateTime>(d => d > DateTime.MinValue), bimestre))
                .ReturnsAsync(true);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferenciaMinValue, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);

            _periodoFechamentoUseCaseMock.Verify(x => x.Executar(
                turmaCodigo,
                It.Is<DateTime>(d => d > DateTime.MinValue && d <= DateTime.Now),
                bimestre), Times.Once);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_TurmaCodigo_Nulo_Deve_Executar_UseCase()
        {
            string turmaCodigo = null;
            var dataReferencia = new DateTime(2025, 10, 8);
            var bimestre = 4;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(false);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.False(retorno);
            _periodoFechamentoUseCaseMock.Verify(x => x.Executar(null, dataReferencia, bimestre), Times.Once);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_TurmaCodigo_Vazio_Deve_Executar_UseCase()
        {
            var turmaCodigo = string.Empty;
            var dataReferencia = new DateTime(2025, 10, 8);
            var bimestre = 1;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(true);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);
            _periodoFechamentoUseCaseMock.Verify(x => x.Executar(string.Empty, dataReferencia, bimestre), Times.Once);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_Bimestre_Zero_Deve_Executar_UseCase()
        {
            var turmaCodigo = "123456";
            var dataReferencia = new DateTime(2025, 10, 8);
            var bimestre = 0;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(false);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.False(retorno);
            _periodoFechamentoUseCaseMock.Verify(x => x.Executar(turmaCodigo, dataReferencia, 0), Times.Once);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_Bimestre_Negativo_Deve_Executar_UseCase()
        {
            var turmaCodigo = "123456";
            var dataReferencia = new DateTime(2025, 10, 8);
            var bimestre = -1;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(true);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);
            _periodoFechamentoUseCaseMock.Verify(x => x.Executar(turmaCodigo, dataReferencia, -1), Times.Once);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_PeriodoFechamentoUseCase_Lanca_Excecao_Deve_Propagar_Excecao()
        {
            var turmaCodigo = "123456";
            var dataReferencia = new DateTime(2025, 10, 8);
            var bimestre = 2;
            var excecaoEsperada = new Exception("Erro no UseCase");

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ThrowsAsync(excecaoEsperada);

            var excecao = await Assert.ThrowsAsync<Exception>(() =>
                _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object));

            Assert.Equal(excecaoEsperada.Message, excecao.Message);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_DataReferencia_Futura_Deve_Executar_UseCase()
        {
            var turmaCodigo = "789012";
            var dataReferencia = new DateTime(2026, 12, 31);
            var bimestre = 4;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(false);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.False(retorno);
            _periodoFechamentoUseCaseMock.Verify(x => x.Executar(turmaCodigo, dataReferencia, bimestre), Times.Once);
        }

        [Fact]
        public async Task PeriodoTurmaAberto_Quando_DataReferencia_Passada_Deve_Executar_UseCase()
        {
            var turmaCodigo = "345678";
            var dataReferencia = new DateTime(2020, 1, 1);
            var bimestre = 1;

            _periodoFechamentoUseCaseMock
                .Setup(x => x.Executar(turmaCodigo, dataReferencia, bimestre))
                .ReturnsAsync(true);

            var resultado = await _controller.PeriodoTurmaAberto(turmaCodigo, bimestre, dataReferencia, _periodoFechamentoUseCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);
            _periodoFechamentoUseCaseMock.Verify(x => x.Executar(turmaCodigo, dataReferencia, bimestre), Times.Once);
        }
    }
}