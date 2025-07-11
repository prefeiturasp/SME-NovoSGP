using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class AcompanhamentoAlunoControllerTeste
    {
        private readonly AcompanhamentoAlunoController _controller;
        private readonly Faker _faker;

        public AcompanhamentoAlunoControllerTeste()
        {
            _controller = new AcompanhamentoAlunoController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar caso de uso para salvar o acompanhamento do aluno")]
        public async Task DeveChamarUseCase_ParaSalvarAcompanhamentoAluno()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarAcompanhamentoAlunoUseCase>();
            var dto = new AcompanhamentoAlunoDto();
            var retorno = new AcompanhamentoAlunoSemestreAuditoriaDto();

            useCaseMock.Setup(u => u.Executar(dto)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.Salvar(useCaseMock.Object, dto);

            // Assert
            useCaseMock.Verify(u => u.Executar(dto), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter o acompanhamento do aluno")]
        public async Task DeveChamarUseCase_ParaObterAcompanhamentoAluno()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAcompanhamentoAlunoUseCase>();
            var turmaId = _faker.Random.Long(1);
            var alunoId = _faker.Random.AlphaNumeric(8);
            var semestre = _faker.Random.Int(1, 2);
            var componenteCurricularId = _faker.Random.Long(1);
            var retorno = new AcompanhamentoAlunoTurmaSemestreDto();

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroAcompanhamentoTurmaAlunoSemestreDto>(f =>
                f.TurmaId == turmaId &&
                f.AlunoId == alunoId &&
                f.Semestre == semestre
            ))).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterAcompanhamentoAluno(turmaId, alunoId, semestre, componenteCurricularId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<FiltroAcompanhamentoTurmaAlunoSemestreDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para deletar foto do aluno")]
        public async Task DeveChamarUseCase_ParaDeletarFoto()
        {
            // Arrange
            var useCaseMock = new Mock<IExcluirFotoAlunoUseCase>();
            var codigoFoto = Guid.NewGuid();
            var retorno = new AuditoriaDto();

            useCaseMock.Setup(u => u.Executar(codigoFoto)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.DeletarFotos(codigoFoto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(codigoFoto), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para upload de foto")]
        public async Task DeveChamarUseCase_ParaUploadDeFoto()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarFotoAcompanhamentoAlunoUseCase>();
            var retorno = new AuditoriaDto();

            var conteudo = "Arquivo de teste";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(conteudo));
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(a => a.Length).Returns(stream.Length);

            var dto = new AcompanhamentoAlunoDto { File = arquivoMock.Object };

            useCaseMock.Setup(u => u.Executar(dto)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.UploadFoto(dto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(dto), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve retornar BadRequest quando a foto para upload estiver vazia")]
        public async Task DeveRetornarBadRequest_QuandoFotoParaUploadEstiverVazia()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarFotoAcompanhamentoAlunoUseCase>();
            var arquivoMock = new Mock<IFormFile>();
            arquivoMock.Setup(a => a.Length).Returns(0);
            var dto = new AcompanhamentoAlunoDto { File = arquivoMock.Object };

            // Act
            var resultado = await _controller.UploadFoto(dto, useCaseMock.Object);

            // Assert
            Assert.IsType<BadRequestResult>(resultado);
            useCaseMock.Verify(u => u.Executar(It.IsAny<AcompanhamentoAlunoDto>()), Times.Never);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter fotos do semestre")]
        public async Task DeveChamarUseCase_ParaObterFotosDoSemestre()
        {
            // Arrange
            var useCaseMock = new Mock<IObterFotosSemestreAlunoUseCase>();
            var acompanhamentoAlunoSemestreId = _faker.Random.Long(1);
            var retorno = new List<ArquivoDto>();

            useCaseMock.Setup(u => u.Executar(acompanhamentoAlunoSemestreId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterFotos(acompanhamentoAlunoSemestreId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(acompanhamentoAlunoSemestreId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter validação de percurso")]
        public async Task DeveChamarUseCase_ParaObterValidacaoPercurso()
        {
            // Arrange
            var useCaseMock = new Mock<IObterValidacaoPercusoRAAUseCase>();
            var turmaId = _faker.Random.Long(1);
            var semestre = _faker.Random.Int(1, 2);
            var retorno = new InconsistenciaPercursoRAADto();

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroInconsistenciaPercursoRAADto>(f => f.TurmaId == turmaId && f.Semestre == semestre)))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterValidacaoPercurso(turmaId, semestre, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<FiltroInconsistenciaPercursoRAADto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}