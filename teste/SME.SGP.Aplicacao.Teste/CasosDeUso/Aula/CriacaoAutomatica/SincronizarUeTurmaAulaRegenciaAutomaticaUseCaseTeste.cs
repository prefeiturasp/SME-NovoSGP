using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;
using Newtonsoft.Json;
using System.Threading;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.CriacaoAutomatica
{
    public class SincronizarUeTurmaAulaRegenciaAutomaticaUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoDadosValidos()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var cacheMock = new Mock<IRepositorioCache>();

            var mensagemRabbit = new MensagemRabbit("chave-cache-exemplo");

            var dadosTurmas = new List<DadosTurmaAulasAutomaticaDto>
            {
                new DadosTurmaAulasAutomaticaDto
                {
                    TurmaCodigo = "1234",
                    ComponenteCurricularCodigo = "5678",
                    ComponenteCurricularDescricao = "Matemática"
                }
            };

            var dadosCriacaoDto = new DadosCriacaoAulasAutomaticasDto(
                "UE123",
                tipoCalendarioId: 1,
                diasLetivosENaoLetivos: new List<DiaLetivoDto>(),
                diasForaDoPeriodoEscolar: new List<DateTime>(),
                modalidade: Modalidade.Fundamental,
                dadosTurmas: dadosTurmas);

            var json = JsonConvert.SerializeObject(dadosCriacaoDto);

            cacheMock.Setup(c => c.ObterAsync("chave-cache-exemplo", false)).ReturnsAsync(json);
            cacheMock.Setup(c => c.RemoverAsync("chave-cache-exemplo")).Returns(Task.CompletedTask);


            cacheMock.Setup(c => c.SalvarAsync(
                 It.IsAny<string>(),
                 It.IsAny<object>(),
                 It.Is<int>(x => x == 300),
                 It.Is<bool>(x => x == false)
             )).Returns(Task.CompletedTask);



            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessorTitularPorTurmaEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProfessorTitularDisciplinaEol { ProfessorRf = "1234567" });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1234" });

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new SincronizarUeTurmaAulaRegenciaAutomaticaUseCase(mediatorMock.Object, cacheMock.Object);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);

            cacheMock.Verify(c => c.RemoverAsync("chave-cache-exemplo"), Times.Once);
            cacheMock.Verify(c => c.SalvarAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.Is<int>(x => x == 300),
                It.Is<bool>(x => x == false)
            ), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
