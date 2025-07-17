using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.CriacaoAutomatica
{
    public class CriarAulasInfantilAutomaticamenteUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveCriarAulasInfantilComSucesso()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var cacheMock = new Mock<IRepositorioCache>();

            var useCase = new CriarAulasInfantilAutomaticamenteUseCase(mediatorMock.Object, cacheMock.Object);

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(new DadosCriacaoAulasAutomaticasCarregamentoDto { Pagina = 1 })
            };

            var tipoCalendarioId = 99;
            var anoLetivo = DateTime.Now.Year;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterExecutarManutencaoAulasInfantilQuery>(), default))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>(), default))
                        .ReturnsAsync(tipoCalendarioId);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), default))
                        .ReturnsAsync(new List<Dominio.PeriodoEscolar> { new Dominio.PeriodoEscolar() });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiasForaDoPeriodoEscolarQuery>(), default))
                        .ReturnsAsync(new List<DateTime>());

            var turma = new SME.SGP.Dominio.Turma
            {
                CodigoTurma = "T123",
                Ue = new Ue { CodigoUe = "UE001" }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilNaoDeProgramaQuery>(), default))
                        .ReturnsAsync(new List<SME.SGP.Dominio.Turma> { turma });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery>(), default))
                        .ReturnsAsync(new List<DiaLetivoDto> { new DiaLetivoDto { Data = DateTime.Today, EhLetivo = true } });

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                        .ReturnsAsync(true);

            cacheMock.Setup(c => c.SalvarAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                300,
                false
            )).Returns(Task.CompletedTask);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterExecutarManutencaoAulasInfantilQuery>(), default), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>(), default), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default), Times.AtLeastOnce);
            cacheMock.Verify(c => c.SalvarAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                300,
                false
            ), Times.AtLeastOnce);
        }
    }
}
