using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;
using BadRequestResult = Microsoft.AspNetCore.Mvc.BadRequestResult;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ArmazenamentoControllerTeste
    {
        private readonly ArmazenamentoController _controller;
        private readonly Faker _faker;

        public ArmazenamentoControllerTeste()
        {
            _controller = new ArmazenamentoController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso quando arquivo for vazio")]
        public async Task Upload_QuandoArquivoForVazio()
        {
            // Arrange
            var mockUseCase = new Mock<IUploadDeArquivoUseCase>();

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act
            var resultado = await _controller.Upload(fileMock.Object, mockUseCase.Object);

            // Assert
            Assert.IsType<BadRequestResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso quando arquivo for válido")]
        public async Task Upload_QuandoArquivoForValido()
        {
            // Arrange
            var mockUseCase = new Mock<IUploadDeArquivoUseCase>();

            var conteudoArquivo = _faker.Lorem.Paragraph();
            var nomeArquivo = _faker.System.FileName("txt");
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(conteudoArquivo));
            var formFile = new FormFile(stream, 0, stream.Length, "file", nomeArquivo)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            var guidEsperado = Guid.NewGuid();

            mockUseCase
                .Setup(u => u.Executar(It.IsAny<IFormFile>(), It.IsAny<TipoArquivo>()))
                .ReturnsAsync(guidEsperado);

            // Act
            var resultado = await _controller.Upload(formFile, mockUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(guidEsperado, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso quando arquivo for nulo")]
        public async Task Download_SeArquivoForNulo()
        {
            // Arrange
            var mockUseCase = new Mock<IDownloadDeArquivoUseCase>();
            var codigoArquivo = Guid.NewGuid();

            byte[] arquivo = null;
            string contentType = null;
            string nomeArquivo = null;

            mockUseCase
                .Setup(u => u.Executar(codigoArquivo))
                .ReturnsAsync((arquivo, contentType, nomeArquivo));

            // Act
            var resultado = await _controller.Download(codigoArquivo, mockUseCase.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso quando arquivo for válido")]
        public async Task Download_SeArquivoForValido()
        {
            // Arrange
            var mockUseCase = new Mock<IDownloadDeArquivoUseCase>();
            var codigoArquivo = Guid.NewGuid();

            var conteudoArquivo = _faker.Random.Bytes(100);
            var contentType = "application/pdf";
            var nomeArquivo = _faker.System.FileName("pdf");

            mockUseCase
                .Setup(u => u.Executar(codigoArquivo))
                .ReturnsAsync((conteudoArquivo, contentType, nomeArquivo));

            // Act
            var resultado = await _controller.Download(codigoArquivo, mockUseCase.Object);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(resultado);
            Assert.Equal(conteudoArquivo, fileResult.FileContents);
            Assert.Equal(contentType, fileResult.ContentType);
            Assert.Equal(nomeArquivo, fileResult.FileDownloadName);
        }

        [Fact(DisplayName = "Deve retornar Ok com o resultado do use case ao excluir o arquivo")]
        public async Task Delete_ComResultadoDoUseCase()
        {
            // Arrange
            var mockUseCase = new Mock<IExcluirArquivoUseCase>();
            var codigoArquivo = Guid.NewGuid();
            var resultadoEsperado = true;

            mockUseCase
                .Setup(u => u.Executar(codigoArquivo))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.Delete(codigoArquivo, mockUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
    }
}
