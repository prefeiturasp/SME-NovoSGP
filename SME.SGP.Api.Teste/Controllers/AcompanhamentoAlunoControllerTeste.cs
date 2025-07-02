using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using Xunit;

namespace SME.SGP.Api.Tests.Controllers
{
    public class AcompanhamentoAlunoControllerTeste
    {
        [Fact]
        public async Task Salvar_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarAcompanhamentoAlunoUseCase>();
            var dto = new AcompanhamentoAlunoDto();

            var resultadoEsperado = new AcompanhamentoAlunoSemestreAuditoriaDto
            {
                AcompanhamentoAlunoId = 1,
                AcompanhamentoAlunoSemestreId = 2,
                Auditoria = new AuditoriaDto()
            };

            useCaseMock
                .Setup(x => x.Executar(dto))
                .ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoAlunoController();

            // Act
            var resultado = await controller.Salvar(useCaseMock.Object, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task ObterAcompanhamentoAluno_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAcompanhamentoAlunoUseCase>();
            long turmaId = 1;
            string alunoId = "123";
            int semestre = 2;
            long componenteCurricularId = 3;
            var resultadoEsperado = new AcompanhamentoAlunoTurmaSemestreDto();

            useCaseMock
                .Setup(x => x.Executar(It.IsAny<FiltroAcompanhamentoTurmaAlunoSemestreDto>()))
                .ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoAlunoController();

            // Act
            var resultado = await controller.ObterAcompanhamentoAluno(turmaId, alunoId, semestre, componenteCurricularId, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task DeletarFotos_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirFotoAlunoUseCase>();
            Guid codigoFoto = Guid.NewGuid();
            var resultadoEsperado = new AuditoriaDto();

            useCaseMock.Setup(x => x.Executar(codigoFoto)).ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoAlunoController();

            // Act
            var resultado = await controller.DeletarFotos(codigoFoto, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task UploadFoto_DeveRetornarOkResult_SeArquivoNaoVazio()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarFotoAcompanhamentoAlunoUseCase>();
            var dto = new AcompanhamentoAlunoDto
            {
                File = new FakeFormFile(1024)  // Mock de arquivo com tamanho > 0
            };

            var resultadoEsperado = new AuditoriaDto
            {
                Id = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "UsuarioTeste"
            };

            useCaseMock
                .Setup(x => x.Executar(dto))
                .ReturnsAsync(resultadoEsperado);  // Aqui está o ponto chave: o método retorna apenas 1 AuditoriaDto

            var controller = new AcompanhamentoAlunoController();

            // Act
            var resultado = await controller.UploadFoto(dto, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }


        [Fact]
        public async Task UploadFoto_DeveRetornarBadRequest_SeArquivoVazio()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarFotoAcompanhamentoAlunoUseCase>();
            var dto = new AcompanhamentoAlunoDto
            {
                File = new FakeFormFile(0) // Arquivo de tamanho 0
            };

            var controller = new AcompanhamentoAlunoController();

            // Act
            var resultado = await controller.UploadFoto(dto, useCaseMock.Object);

            // Assert
            Assert.IsType<BadRequestResult>(resultado);
        }

        [Fact]
        public async Task ObterFotos_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<IObterFotosSemestreAlunoUseCase>();
            long acompanhamentoAlunoSemestreId = 1;
            var resultadoEsperado = new List<ArquivoDto>();

            useCaseMock.Setup(x => x.Executar(acompanhamentoAlunoSemestreId)).ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoAlunoController();

            // Act
            var resultado = await controller.ObterFotos(acompanhamentoAlunoSemestreId, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task ObterValidacaoPercurso_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<IObterValidacaoPercusoRAAUseCase>();
            long turmaId = 1;
            int semestre = 2;
            var resultadoEsperado = new InconsistenciaPercursoRAADto();

            useCaseMock
                .Setup(x => x.Executar(It.IsAny<FiltroInconsistenciaPercursoRAADto>()))
                .ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoAlunoController();

            // Act
            var resultado = await controller.ObterValidacaoPercurso(turmaId, semestre, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
    }

    // Mock de IFormFile simples (já que o DTO tem um File)
    public class FakeFormFile : Microsoft.AspNetCore.Http.IFormFile
    {
        private readonly long _length;

        public FakeFormFile(long length)
        {
            _length = length;
        }

        public string ContentType => throw new NotImplementedException();
        public string ContentDisposition => throw new NotImplementedException();
        public IHeaderDictionary Headers => throw new NotImplementedException();
        public long Length => _length;
        public string Name => throw new NotImplementedException();
        public string FileName => throw new NotImplementedException();
        public void CopyTo(System.IO.Stream target) => throw new NotImplementedException();
        public Task CopyToAsync(System.IO.Stream target, System.Threading.CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public System.IO.Stream OpenReadStream() => throw new NotImplementedException();
    }
}
