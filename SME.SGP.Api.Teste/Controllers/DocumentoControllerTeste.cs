using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class DocumentoControllerTeste
    {
        [Fact]
        public async Task ValidacaoUsuario_DeveRetornarOk()
        {
            var mockUseCase = new Mock<IVerificarUsuarioDocumentoUseCase>();
            var expected = true;

            mockUseCase
                .Setup(x => x.Executar(It.IsAny<VerificarUsuarioDocumentoDto>()))
                .ReturnsAsync(expected);

            var controller = new DocumentoController();

            var result = await controller.ValidacaoUsuario(1, 1, 1, 1, 1, 2025, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task ListarTiposDeDocumentos_DeveRetornarOk()
        {
            var mockUseCase = new Mock<IListarTiposDeDocumentosUseCase>();

            var expected = new List<TipoDocumentoDto>
            {
                new TipoDocumentoDto { Id = 1, TipoDocumento = "Relatório" },
                new TipoDocumentoDto { Id = 2, TipoDocumento = "Comunicado" }
            };

            mockUseCase
                .Setup(x => x.Executar())
                .ReturnsAsync(expected);

            var controller = new DocumentoController();

            var result = await controller.ListarTiposDeDocumentos(mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<TipoDocumentoDto>>(okResult.Value);
            Assert.Equal(2, ((List<TipoDocumentoDto>)value).Count);
        }

        [Fact]
        public async Task SalvarDocumento_DeveRetornarOk()
        {
            var mockUseCase = new Mock<ISalvarDocumentoUseCase>();

            var dto = new SalvarDocumentoDto(
                arquivosCodigos: new Guid[] { Guid.NewGuid() },
                ueId: 1L,
                tipoDocumentoId: 10L,
                classificacaoId: 20L,
                usuarioId: 30L
            );

            mockUseCase.Setup(x => x.Executar(It.IsAny<SalvarDocumentoDto>()))
                       .ReturnsAsync(true);

            var controller = new DocumentoController();

            var result = await controller.SalvarDocumento(dto, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);

            mockUseCase.Verify(x => x.Executar(It.IsAny<SalvarDocumentoDto>()), Times.Once());
        }

        [Fact]
        public async Task UploadDocumentos_DeveRetornarOk()
        {
            var mockUseCase = new Mock<IUploadDocumentoUseCase>();
            var fileMock = new Mock<IFormFile>();
            var content = "Arquivo de Teste";
            var fileName = "arquivo.txt";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            var expectedGuid = Guid.NewGuid();
            mockUseCase.Setup(x => x.Executar(fileMock.Object)).ReturnsAsync(expectedGuid);

            var controller = new DocumentoController();

            var result = await controller.UploadDocumentos(fileMock.Object, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedGuid, okResult.Value);
        }

        [Fact]
        public async Task UploadDocumentos_DeveRetornarBadRequest_QuandoArquivoVazio()
        {
            var mockUseCase = new Mock<IUploadDocumentoUseCase>();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            var controller = new DocumentoController();

            var result = await controller.UploadDocumentos(fileMock.Object, mockUseCase.Object);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ExcluirDocumento_DeveRetornarOk()
        {
            var mockUseCase = new Mock<IExcluirDocumentoUseCase>();
            mockUseCase.Setup(x => x.Executar(1)).ReturnsAsync(true);

            var controller = new DocumentoController();

            var result = await controller.ExcluirDocumento(1, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }
    }
}