using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.FluenciaLeitora;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Ideb;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ImportarArquivo.Idep;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ImportarArquivoControllerTeste
    {
        private readonly ImportarArquivoController _controller;
        private readonly Mock<IImportacaoArquivoIdebUseCase> _importacaoArquivoIdebUseCase;
        private readonly Mock<IImportacaoArquivoIdepUseCase> _importacaoArquivoIdepUseCase;
        private readonly Mock<IImportacaoArquivoFluenciaLeitoraUseCase> _importacaoArquivoFluenciaLeitoraUseCase;
        private readonly Mock<IImportacaoLogUseCase> _importacaoLogUseCase;
        private readonly Mock<IImportacaoLogErroUseCase> _importacaoLogErroUseCase;
        private readonly Faker _faker;

        public ImportarArquivoControllerTeste()
        {
            _importacaoArquivoIdebUseCase = new Mock<IImportacaoArquivoIdebUseCase>();
            _importacaoArquivoIdepUseCase = new Mock<IImportacaoArquivoIdepUseCase>();
            _importacaoArquivoFluenciaLeitoraUseCase = new Mock<IImportacaoArquivoFluenciaLeitoraUseCase>();
            _importacaoLogUseCase = new Mock<IImportacaoLogUseCase>();
            _importacaoLogErroUseCase = new Mock<IImportacaoLogErroUseCase>();
            _faker = new Faker();
            _controller = new ImportarArquivoController();
        }

        #region ImportarArquivoIdeb
        [Fact]
        public async Task ImportarArquivoIdeb_DeveRetornarOk_ComResultadoEsperado()
        {
            // Arrange
            var arquivoMock = new Mock<IFormFile>();
            var anoLetivo = DateTime.Now.Year;
            var resultadoEsperado = new ImportacaoLogRetornoDto { Sucesso = true, Mensagem = MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO };

            _importacaoArquivoIdebUseCase
                .Setup(u => u.Executar(arquivoMock.Object, anoLetivo))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ImportarArquivoIdeb(arquivoMock.Object, anoLetivo, _importacaoArquivoIdebUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

        #region ImportarArquivoIdep
        [Fact]
        public async Task ImportarArquivoIdep_DeveRetornarOk_ComResultadoEsperado()
        {
            // Arrange
            var arquivoMock = new Mock<IFormFile>();
            var anoLetivo = DateTime.Now.Year;
            var resultadoEsperado = new ImportacaoLogRetornoDto { Sucesso = true, Mensagem = MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO };

            _importacaoArquivoIdepUseCase
                .Setup(u => u.Executar(arquivoMock.Object, anoLetivo))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ImportarArquivoIdep(arquivoMock.Object, anoLetivo, _importacaoArquivoIdepUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

        #region ImportarArquivoFluenciaLeitora
        [Fact]
        public async Task ImportarArquivoFluenciaLeitora_DeveRetornarOk_ComResultadoEsperado()
        {
            // Arrange
            var arquivoMock = new Mock<IFormFile>();
            var anoLetivo = DateTime.Now.Year;
            var tipoAvaliacao = 1;
            var resultadoEsperado = new ImportacaoLogRetornoDto { Sucesso = true, Mensagem = MensagemNegocioComuns.ARQUIVO_IMPORTADO_COM_SUCESSO };

            _importacaoArquivoFluenciaLeitoraUseCase
                .Setup(u => u.Executar(arquivoMock.Object, anoLetivo, tipoAvaliacao))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ImportarArquivoFluenciaLeitora(arquivoMock.Object, anoLetivo, tipoAvaliacao, _importacaoArquivoFluenciaLeitoraUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

        #region ObterImportacaoLog
        [Fact]
        public async Task ObterImportacaoLog_DeveRetornarOk_ComResultadoEsperado()
        {
            // Arrange
            var filtro = new FiltroPesquisaImportacaoDto();
            var resultadoEsperado = new PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>();

            _importacaoLogUseCase
                .Setup(u => u.Executar(
                    It.IsAny<FiltroPesquisaImportacaoDto>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ObterImportacaoLog(filtro, _importacaoLogUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<PaginacaoResultadoDto<ImportacaoLogQueryRetornoDto>>(okResult.Value);

            Assert.Same(resultadoEsperado, retorno);
        }
        #endregion

        #region ObterImportacaoLogErro
        [Fact]
        public async Task ObterImportacaoLogErros_DeveRetornarOk_ComResultadoEsperado()
        {
            // Arrange
            var filtro = new FiltroPesquisaImportacaoDto();
            var resultadoEsperado = new PaginacaoResultadoDto<ImportacaoLogErroQueryRetornoDto>();

            _importacaoLogErroUseCase
                .Setup(u => u.Executar(filtro))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ObterImportacaoLogErros(filtro, _importacaoLogErroUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

    }
}
